
namespace AdventOfCode2025
{
	public class Day05
	{
		public int First(IList<string> input)
		{
			var result = 0;
			var ingredienceDatabse = this.ParseInput(input);
            foreach(var id in ingredienceDatabse.numbers)
            {
                if(IsFresh(id, ingredienceDatabse.ranges))
                {
                    result++;
                }
            }

			return result;
		}
        public bool IsFresh(long id,IList<(long start, long end)> ranges)
        {
            foreach(var range in ranges)
            {
                if(id >= range.start && id <= range.end)
                {
                    return true;

                }
            }
            return false;
        }

        public (long start, long end) MergeRange((long start, long end) range1, (long start, long end)range2)
        {
            return (Math.Min(range1.start, range2.start), Math.Max(range1.end, range2.end));
        }
        public bool IsOverlapping((long start, long end) range1, (long start, long end)range2)
        {
            return range1.start >= range2.start && range1.start <= range2.end
                || range1.end >= range2.start && range1.end <= range2.end
            ;
        }
        public bool AreEqual((long start, long end) range1, (long start, long end)range2)
        {
            return range1.start == range2.start && range1.end == range2.end
            ;
        }

		public long Second(IList<string> input)
		{
			long result = 0;
			var ranges = this.ParseInput(input).ranges.ToList();
            var merged = true;
            while(merged)
            {
                merged = false;
                for(int i = 0;i <  ranges.Count; i++)
                {
                    for(int j = 0; j <  ranges.Count; j++)
                    {
                        var first = ranges[i];
                        var second = ranges[j];
                        if(IsOverlapping(first,second) && !AreEqual(first,second))
                        {
                            ranges.Remove(first);
                            ranges.Remove(second);
                            ranges.Add(MergeRange(first,second));
                            merged = true;
                            break;
                        }
                    }
                }
            }
            foreach(var range in ranges
                .GroupBy(x => x).Select(x => x.First())
                )
            {
                result += range.end - range.start + 1;
            }
            return result;
		}

		public (IList<(long start, long end)> ranges, IList<long> numbers) ParseInput(IList<string> inputs)
		{
            var ranges = new List<(long start, long end)>();
            var numbers = new List<long>();
			foreach (var input in inputs.Where(x => !string.IsNullOrEmpty(x)))
            {
                var parsed = input.Split('-').ToList();
                if(parsed.Count>1)
                {
                    ranges.Add((long.Parse(parsed[0]), long.Parse(parsed[1])));
                }
                else
                {
                    numbers.Add(long.Parse(parsed[0]));
                }
            }
			return (ranges, numbers);
		}
	}
}
