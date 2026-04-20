namespace AdventOfCode2025
{
	public enum Part{ One,Two }
	public class ParallelUtils
	{
		public static void SingleWhile(Func<bool> condition, Action body)
		{
			do
			{
				body();
			}
			while (condition());
		}
		public static void While(Func<bool> condition, Action body)
		{
			Parallel.ForEach(IterateUntilFalse(condition)
				, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }
				, ignored => body()
				);
		}

		private static IEnumerable<bool> IterateUntilFalse(Func<bool> condition)
		{
			while (condition())
			{
				yield return true;
			}
		}
	}
	public class SingleValueHolder
	{
		private int _value;

		public SingleValueHolder(int initialValue)
		{
			_value = initialValue;
		}

		// Atomic replacement
		public void TryLower(int newValue)
		{
			int current;
			do
			{
				current = Volatile.Read(ref _value);
				if (newValue >= current)
				{
					return;
				}
			}
			while (Interlocked.CompareExchange(ref _value, newValue, current) != current);
		}

		// Atomic read
		public int Get()
		{
			return Volatile.Read(ref _value);
		}
	}

	internal class Helper
	{
		public static List<(int y, int x)> GetAdjacent((int x, int y) p) => GetAdjacent(p.x, p.y);
		public static List<(int y, int x)> GetAdjacent(int x, int y)
		{
			return new List<(int y, int x)>
			{
				(y-1,x-1),(y-1,x),(y-1,x+1),
				(y,x-1), (y,x+1),
				(y+1,x-1),(y+1,x),(y+1,x+1)
			};
		}
		public static bool InBounds((int x, int y) pos, int xMax, int yMax)
		{
			return InBounds(pos.x, pos.y, xMax, yMax);
		}
		public static bool InBounds(int x, int y, int xMax, int yMax)
		{
			return x >= 0 && y >= 0 && x < xMax && y < yMax;
		}
		
	}
	internal static class Extentions
	{
		public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
		{
			foreach (T t in values) 
			{
				set.Add(t);
			}
		}
	}
}
