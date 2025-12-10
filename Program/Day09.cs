namespace AdventOfCode2025
{

	/*
	 This day make me feel bad..

Find the biggest square inside the shape from one # to any other #
Real example was 100000*100000 in size; 
.......#XXX#.
.......X...X.
..#XXXX#...X.
..X........X.
..#XXXXXX#.X.
.........X.X.
.........#X#.

since only placing a square between any # there could be a big square outside the shape like this.

.......#XXX#.
.......X...X.
..#XXXX#...X.
..X........X.
..OOOOOOOO.X.
..OOOOOOOO.X.
..OOOOOOOOX#.
.............

This made me want to figur out all the positions outside of the shape with floodfill end exclude them. But since the real input is so big
i decided to try and do it in chunks of decreasing size. This worked for the example but was slow for the real problem.

Turns out that the real problem could have been solved from the start by just checking any 2 # and make sure there is no intersection of other ranges.
The real data turned out to not have large areas outside of the shape so i didnt need to think about that scenario..
I wish i figured this out earlier but the example input had this problem so i wanted to sole it for every shape possible, not onle the "easy" one.
I Learned some new tricks with floodfill and data symbolization but with growing frustration.

	 */
	public struct Range
	{
		public (long x, long y) Start;
		public (long x, long y) End;
		public long XMax => Start.x > End.x ? Start.x : End.x;
		public long XMin => Start.x < End.x ? Start.x : End.x;
		public long YMax => Start.y > End.y ? Start.y : End.y;
		public long YMin => Start.y < End.y ? Start.y : End.y;
		public Range((long x, long y) start, (long x, long y) end) => (Start, End) = (start, end);
	}
	public class Day09
	{
	
		public long First(IList<string> input)
		{
			var coordinates = this.ParseInput(input);
			var max = 0L;

			for (int i = 0; i < coordinates.Count; i++)
			{
				for (int j = 0; j < coordinates.Count; j++)
				{
					var area = GetArea(coordinates[i], coordinates[j]);
					if (area > max)
					{
						max = area;
					}
				}
			}
			return max;
		}


		public long Second(IList<string> input)
		{
			var ranges = this.ParseInputPart2(input);
			var coordinates = this.ParseInput(input);

			var maxArea = 0L;
			for (int i = 0; i < coordinates.Count; i++)
			{
				for (int j = i; j < coordinates.Count; j++)
				{
					var first = coordinates[i];
					var second = coordinates[j];
					var area = GetArea(coordinates[i], coordinates[j]);
					var range = new Range(coordinates[i], coordinates[j]);
					if (area > maxArea && !Collition(range, ranges))
					{
						maxArea = area;
						Print(ranges,coordinates.ToHashSet(),range);
					}
				}
			}
			return maxArea;
		}
		public bool Collition(Range tester, IList<Range> ranges)
		{
			for (int i = 0; i < ranges.Count;i++)
			{
				var range = ranges[i];
				long rMinX = range.XMin;
				long rMaxX = range.XMax;
				long rMinY = range.YMin;
				long rMaxY = range.YMax;
				if (tester.XMin < rMaxX && tester.XMax > rMinX && tester.YMin < rMaxY && tester.YMax > rMinY)
					{
					return true;
				}
			}
			return false;
		}
		public void Print(IList<Range> ranges, HashSet<(long x, long y)> original, Range testSquare)
		{
			var set = new HashSet<(long x, long y)>();
			var testSquarePosition = this.GetAreaPositions(testSquare);
			(long xMax, long yMax) max = (12, 12);
			foreach (var range in ranges)
			{
				set.AddRange(GetAreaPositions(range));
			}
			for (long y = 0; y <= max.yMax; y++)
			{
				for (long x = 0; x <= max.xMax; x++)
				{
					var character = set.Contains((x, y)) ? 'X' : '.';
					if (original.Contains((x, y)))
					{
						character = '#';
					}
					if(testSquarePosition.Any(t => t == (x,y)))
					{
						character = 'O';
					}
					Console.Write(character);
				}
				Console.WriteLine();
			}
		}

		public IEnumerable<(long x, long y)> GetAreaPositions(Range range)
		{
			
			for (long y = range.YMin; y <= range.YMax; y++)
			{
				for (long x = range.XMin; x <= range.XMax; x++)
				{
					yield return (x, y);
				}
			}
		}

		public long GetArea((long x, long y) pos1, (long x, long y) pos2)
		{
			long dX = Math.Abs(pos1.x - pos2.x) + 1;
			long dY = Math.Abs(pos1.y - pos2.y) + 1;
			return (long)dX * (long)dY;
		}

		public List<Range> ParseInputPart2(IList<string> input)
		{
			var values = new List<Range>();
			for (int i = 0; i < input.Count - 1; i++)
			{
				var start = input[i].Split(',').Select(x => long.Parse(x)).ToList();
				var end = input[i + 1].Split(',').Select(x => long.Parse(x)).ToList();
				values.Add(new Range((start[0], start[1]), (end[0], end[1])));
			}

			var last = input[input.Count - 1].Split(',').Select(x => long.Parse(x)).ToList();
			var first = input[0].Split(',').Select(x => long.Parse(x)).ToList();
			values.Add(new Range((last[0], last[1]), (first[0], first[1])));

			return values;
		}

		public List<(long x, long y)> ParseInput(IList<string> input)
		{
			var values = new List<(long x, long y)>();

			foreach (var line in input)
			{
				var numbers = line.Split(',');
				values.Add((long.Parse(numbers[0]), long.Parse(numbers[1])));
			}
			return values;
		}
	}
}