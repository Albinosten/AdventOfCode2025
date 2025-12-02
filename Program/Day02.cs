namespace AdventOfCode2025
{
    public class Day02
    {
        /*
            First i did this solution but was thinking there might be another way of doing it so i tried it with regex.
            Decided not to go with regex since performance wise my code was just as good or better.

            Ran every range 100times extra and got these times.
            My code:
            part1 = 12s
            part2 = 2m19s
            with regex:
            part1 1m54s
            part2 = 2m 19s

            Conclution: Regex looks cooler but it might not always be the fastest.
        */
        public long First(IList<string> input)
        {
            var ranges = this.ParseInput(input);
            var result = 0L;
            foreach(var range in ranges)
            {
                for(long number = range.start; number <= range.end; number++)
                {
                    var numberString = number.ToString();
                    if(StringCompare(numberString)) //Regex.IsMatch(numberString, "^(.+)\\1$")
                    {
                        result +=number;
                    }
                }
            }
            
            return result;
        }
        public long Second(IList<string> input)
        {
            var ranges = this.ParseInput(input);
            var result = 0L;
            foreach(var range in ranges)
            {
                for(long number = range.start; number <= range.end; number++)
                {
                    if(IsInvalid(number)) // Regex.IsMatch(number.ToString(), "^(.+)\\1+$")
                    {
                        result += number;
                    }
                }
            }
            
            return result;
        }
        public bool StringCompare(string number)
        {
            if(number.Length % 2 != 0)
            {
                return false;
            }
            
            var halfLength = number.Length/2;
            for(int i = 0; i < halfLength;i++)
            {
                if(number[i] != number[i+halfLength])
                {
                    return false;
                }
            }
            
            return true;
        }

        public bool AllAreEqual(IEnumerable<char[]> input)
        {
            char[]? first = null;
            foreach(var row in input)
            {
                first = first ?? row;
                if(row.Length != first.Length){return false;}       
                for(int i = 0; i < row.Length; i++)
                {
                    if(row[i] != first[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public bool IsInvalid(long number)
        {
            var numberString = number.ToString();
            for(int i = 1; i <= numberString.Length / 2; i++)
            {
                if(AllAreEqual(numberString.Chunk(i)))
                {
                    return true;    
                }
            }
            return false;
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