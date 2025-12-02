namespace AdventOfCode2025
{
    public class Day02
    {
        public long First(IList<string> input)
        {
            var ranges = this.ParseInput(input);
            var invalids = new List<long>();
            foreach(var range in ranges)
            {
                for(long number = range.start; number <= range.end; number++)
                {
                    var numberString = number.ToString();
                    var first = numberString.Substring(0,numberString.Length/2);
                    var second = numberString.Substring(numberString.Length/2);
                    if(first == second)
                    {
                        invalids.Add(number);
                    }
                }
            }
            
            return invalids.Sum();
        }

        public long Second(IList<string> input)
        {
return 0;
        }
        public IList<(long start, long end)> ParseInput(IList<string> input)
        {
			var values = new List<(long start, long end)>();

			foreach (var line in input)
			{
                foreach(var range in line.Split(','))
                {
                    var numbers = range.Split('-');
                    values.Add((long.Parse(numbers[0]), long.Parse(numbers[1])));
                }
			}
            return values;
        }
    }
}