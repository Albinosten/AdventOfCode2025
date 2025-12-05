namespace AdventOfCode2025
{
    public class Day01
    {
        public int First(IList<string> input)
        {
            var dialStartValue = 50;
            var result = 0;
            foreach(var value in ParseInput(input))
            {
                dialStartValue += value.direction * value.count;
                if(dialStartValue % 100 == 0)
                {
                    result ++;
                }
            }
            return result;
        }
        public int Second(IList<string> input)
        {
            var dialStartValue = 50;
            var result = 0;
            foreach(var value in ParseInput(input))
            {
                for(int i = 0; i < value.count;i++)
                {   
                    dialStartValue += value.direction;
                    if(dialStartValue % 100 == 0)
                    {
                        result ++;
                    }
                }
            }
            return result;
        }
        public IList<(int direction, int count)> ParseInput(IList<string> input)
        {
			var values = new List<(int direction, int count)>();

			foreach (var line in input)
			{
				var direction = line[0] == 'L' ? -1 : 1;
                var value = int.Parse(new string(line.Skip(1).ToArray()));
                values.Add((direction, value));
			}
            return values;
        }
    }
}