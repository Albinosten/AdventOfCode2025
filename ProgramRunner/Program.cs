using AdventOfCode2025;
using System.Diagnostics;
namespace ProgramRunner
{
	internal class ProgramRunner
	{
		static object GetFirstResult(object puzzle) => puzzle switch
		{
			Day01 => 1086,
			Day02 => 52316131093,
			Day03 => 17074,
			Day04 => 1478,
			Day05 => 525, 
			Day06 => 4693419406682,
			Day07 => 1560,
			Day08 => 26400,
			Day09 => 4777967538,
			Day10 => 419,
			Day11 => 543,
			_ => throw new NotImplementedException(),
		};
		static object GetSecondResult(object puzzle) => puzzle switch
		{
			Day01 => 6268,
			Day02 => 69564213293,
			Day03 => 169512729575727,
			Day04 => 9120,
			Day05 => 333892124923577,
			Day06 => 9029931401920,
			Day07 => 25592971184998,
			Day08 => 8199963486,
			Day09 => 1439894345,
			Day10 => 18369,
			Day11 => 479511112939968,

			_ => throw new NotImplementedException(),
		};
		
		static void ValidateResult(object puzzle, object firstResult, object secondResult)
		{
			var firstExpected = GetFirstResult(puzzle);
			if (!Compare(firstExpected,firstResult))
			{
				throw new Exception($"Wrong answer on first part. Expected {firstExpected}: Actual {firstResult}");
			}
			var secondExpected = GetSecondResult(puzzle);
			if(!Compare(secondExpected, secondResult))
			{
				throw new Exception($"Wrong answer on second part. Expected {secondExpected}: Actual {secondExpected}");
			}
		}

		static bool Compare(object a, object b)
		{
			var aS = a.ToString();
			var bS = b.ToString();
			if(aS.Length != bS.Length)
			{
				return false; 
			}
			for(var i = 0; i < aS.Length; i++)
			{
				if(aS[i] != bS[i])
				{
					return false;
				}
			}
			return true;
		}

		static async Task Main(string[] args)
		{

			var puzzles = new[]
			{
				() => RunDay01(),
				() => RunDay02(),
				() => RunDay03(),
				() => RunDay04(),
				() => RunDay05(),
				() => RunDay06(),
				() => RunDay07(),
				() => RunDay08(),
				() => RunDay09(),
				() => RunDay10(),
				() => RunDay11(),
			};

			var i = 1;
			foreach (var puzzle in puzzles) 
			{
				var time = puzzle();
				Console.WriteLine($"Time for puzzle {i} took {time}");
				i++;
			}
		}

		public static TimeSpan RunDay01()
		{
			var puzzle = new Day01();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay02()
		{
			var puzzle = new Day02();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input[0]);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay03()
		{
			var puzzle = new Day03();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start(); 
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay04()
		{
			var puzzle = new Day04();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay05()
		{
			var puzzle = new Day05();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay06()
		{
			var puzzle = new Day06();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay07()
		{
			var puzzle = new Day07();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay08()
		{
			var puzzle = new Day08();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay09()
		{
			var puzzle = new Day09();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay10()
		{
			var puzzle = new Day10();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input, false);
			var secondResult = puzzle.Second(input, false);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
		public static TimeSpan RunDay11()
		{
			var puzzle = new Day11();
			var input = FileReader.GetInput(false, puzzle);
			var clock = new Stopwatch();
			clock.Start();
			var firstResult = puzzle.First(input);
			var secondResult = puzzle.Second(input);
			clock.Stop();
			ValidateResult(puzzle, firstResult, secondResult);
			return clock.Elapsed;
		}
	}
}
