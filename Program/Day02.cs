using System.Collections.Concurrent;

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

            Edit: I want to point out that this was not the official time for my progam. 
            This was with just for timing different solutions. Official time was 1.6s.
            Now the time is 0.38s for both parts. 

            Edit2: I changed from a confusing math approach into something easier...
            New time is 0.09s for both parts.
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
        public long Second(string input)
        {
            var result = new ConcurrentBag<long>();
            var ranges = input
                .Split(',')
                .ToList()
                ;
            Parallel.ForEach(ranges, range => 
            {
                var r = ParseInput(range);
                for(long number = r.start; number <= r.end; number++)
                {
                    if(Validate(number))
                    {
                        result.Add(number);
                    }
                }
            });
            
            return result.Sum();
        }
        public bool AllDigitsAreEqual(long number)
        {
            long lastDigit = number % 10;
            while (number > 0)
            {
                if (number % 10 != lastDigit)
                {
                    return false;
                }

                number /= 10;
            }

            return true;
        }
        public bool Validate(long number)
        {
            if(number / 10 == 0){return false;}
            if(AllDigitsAreEqual(number)){return true;}
            var numberString = number.ToString();
            for(int i = 2; i <= numberString.Length/2; i++)
            {
                if(StringCompare(numberString,i))
                {
                    return true;
                }
            }
            return false;
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

        public bool StringCompare(string number, int patternSize)
        {   
            if(number.Length % patternSize != 0 ){return false;}
            for(int i = 0; i < number.Length - patternSize; i++)
            {
                var firstValue = number[i];
                var secondValue = number[i + patternSize];
                if(firstValue != secondValue)
                {
                    return false;
                }
            }
            
            return true;
        }

        public (long start, long end) ParseInput(string input)
        {
            var numbers = input.Split('-');
            return (long.Parse(numbers[0]), long.Parse(numbers[1]));
        }
        public IList<(long start, long end)> ParseInput(IList<string> input)
        {
			var values = new List<(long start, long end)>();

			foreach (var line in input)
			{
                foreach(var range in line.Split(','))
                {
                    values.Add(ParseInput(range));
                }
			}
            return values;
        }
    }
}