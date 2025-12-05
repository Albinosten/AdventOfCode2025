using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2025
{
	internal class Helper
	{
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
}
