namespace AdventOfCode2025
{
	public class SingleValueHolder
	{
		private int _value;

		public SingleValueHolder(int initialValue)
		{
			_value = initialValue;
		}

		// Atomic replacement
		public void Set(int newValue)
		{
			var min = Math.Min(newValue, Get());
			Interlocked.Exchange(ref _value, min);
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
