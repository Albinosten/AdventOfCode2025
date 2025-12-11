namespace AdventOfCode2025
{
    public class Day11
    {
        public int First(IList<string> input)
        {
            var cables = this.ParseInput(input);
            var cablesDictionary = cables.ToDictionary(x => x.Item1);

            var s = new Stack<string>();
            s.Push("you");

            var count = 0;
            while (s.Count > 0) 
            {
                var l = s.Pop();
                if(l == "out")
                {
                    count++;
                    continue;
                }
                var next = cablesDictionary[l].Item2;
                for(int i = 0; i < next.Count;i++)
                {

                    s.Push(next[i]);
                }
            }

            return count;
        }
        public long Second(IList<string> input)
        {
			var cables = this.ParseInput(input);
			var cablesDictionary = cables.ToDictionary(x => x.Item1);
			cablesDictionary.Add("out", ("out", new List<string>()));//make sure out exist

			var s = new Stack<(string name, bool dac, bool fft, HashSet<string>visited)>();
			s.Push(("svr", false,false, new HashSet<string>()));


			var svrTofft = RunRecursive("svr",cablesDictionary, new Dictionary<string, long>(), "fft");
			var fftTodac = RunRecursive("fft", cablesDictionary, new Dictionary<string, long>(), "dac");
			var dacTofft = RunRecursive("dac", cablesDictionary, new Dictionary<string, long>(), "fft");
			var dacToEnd = RunRecursive("dac", cablesDictionary, new Dictionary<string, long>(), "out");
			return svrTofft*fftTodac*dacToEnd;
		}
		public long RunRecursive(string node, Dictionary<string, (string,List<string>)> cables, Dictionary<string, long> cache, string end)
		{
			if(node == end)
			{
				return 1;
			}
			var key = node;
			if (cache.ContainsKey(key)) 
			{
				return cache[key];
			}
			var total = 0L;
			foreach(var next in cables[node].Item2)
			{
				total += RunRecursive(next, cables, cache, end);
			}
			cache[key] = total;
			return total;
		}
		public IList<(string, List<string>)> ParseInput(IList<string> input)
        {
			var values = new List<(string, List<string>)>();

			foreach (var line in input)
			{
                var start = line.Split(':')[0];
                var outs = line.Split(":")[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                values.Add((start, outs));
			}
            return values;
        }
    }
}