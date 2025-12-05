
namespace AdventOfCode2025
{
	public class Day04
	{
		public int First(IList<string> input)
		{
			var result = 0;
			var warehouse = this.ParseInput(input);
			for(int y = 0; y < warehouse.Count; y++) 
			{
				for (int x = 0; x < warehouse[y].Count; x++)
				{
					if (warehouse[y][x] && Helper
						.GetAdjacent(x, y)
						.Where(r => Helper
							.InBounds(r, warehouse[y].Count, warehouse.Count))
						.Where(x => warehouse[x.y][x.x])
						.Count() < 4)
					{
						result++;
					}
				}
			}
			return result;
		}

		public int Second(IList<string> input)
		{

			var result = 0;
			var warehouse = this.ParseInput(input);
			var anyWasRemoved = false;
			do
			{
				anyWasRemoved = false;
				for (int y = 0; y < warehouse.Count; y++)
				{
					for (int x = 0; x < warehouse[y].Count; x++)
					{
						if (warehouse[y][x] && Helper
							.GetAdjacent(x, y)
							.Where(r => Helper
								.InBounds(r, warehouse[y].Count, warehouse.Count))
							.Where(x => warehouse[x.y][x.x])
							.Count() < 4)
						{
							result++;
							warehouse[y][x] = false;
							anyWasRemoved = true;
						}
					}
				}
			} while (anyWasRemoved);
			return result;
		}

		public IList<IList<bool>> ParseInput(IList<string> input)
		{
			var result = new List<IList<bool>>();
			foreach (var y in input) 
			{
				var row = new List<bool>();
				foreach (var x in y)
				{
					row.Add(x == '@');
				}
				result.Add(row);
			}
			return result;
		}
	}
}
