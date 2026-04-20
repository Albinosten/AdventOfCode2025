using System.Collections.Concurrent;

namespace AdventOfCode2025
{
	public class Day10_New
	{
		public Day10 day10;
		public Day10_New()
		{
			this.day10 = new Day10();
		}

		public long SolveWithConcurrentList(IList<string> input)
		{
			var machines = this.day10.ParseInput(input).ToList();
			var minResult = new Dictionary<int, SingleValueHolder>();
			var movesDictionary = new ConcurrentDictionary<int, ConcurrentDictionary<int, List<int[]>>>();

			var list = new ConcurrentStack<(Machine m
			, int score
			, List<int> usedJoltages
			, int buttonPressedForJoltage
			, int depth
			, List<int> expectedJoltage
			, List<List<int>> Buttons
			, ConcurrentDictionary<string, int> previousJoltDictionary
			, SingleValueHolder minValue
			)> ();

			foreach (var m in machines)
			{
				var minValue = new SingleValueHolder(int.MaxValue);
				list.Push((m, 0, new List<int>(), 0, 0, m.ExpectedJoltage, m.Buttons, new ConcurrentDictionary<string, int>(), minValue));
				minResult.Add(m.Id, minValue);
				movesDictionary.TryAdd(m.Id,new ConcurrentDictionary<int, List<int[]>>());
			}
			do
			{

				ParallelUtils.While(() => !list.IsEmpty, () =>
				{
					if (!list.TryPop(out var o))
					{
						return;
					}
					var machine = o.m;
					if (o.score > o.minValue.Get())
					{
						return;
					}

					var filteredButtons = Machine.FilterMovesExcludePreviousJoltagesMovesThreadSafe(movesDictionary[o.m.Id]
						, o.Buttons
						, o.usedJoltages
						, o.previousJoltDictionary
						);
					if (filteredButtons.joltage == -1)
					{
						return;
					}
					var value = o.expectedJoltage[filteredButtons.joltage] - machine.joltage[filteredButtons.joltage];
					var iMax = o.expectedJoltage[filteredButtons.joltage] - o.buttonPressedForJoltage;
					for (int i = 0; i <= iMax; i++)
					{
						if (i == value)
						{
							var newMachene = machine.SoftClone(false);

							var newButtonPressedForJoltage = o.score;
							if (filteredButtons.Item1.Count > 0)
							{
								newButtonPressedForJoltage += i;
								newMachene.ApplyMultipleMove(i, filteredButtons.Item1[o.depth]);
							}

							var joltageOverloaded = newMachene.joltageOverload(o.expectedJoltage, 0);

							if (!joltageOverloaded && newMachene.joltageIsComplete(o.expectedJoltage, o.expectedJoltage.Count))
							{
								o.minValue.TryLower(o.score + i);
								return;
							}
							else if(!joltageOverloaded)
							{
								list.Push((newMachene
									, newButtonPressedForJoltage
									, [filteredButtons.joltage, .. o.usedJoltages]
									, 0
									, 0
									, o.expectedJoltage
									, o.Buttons
									, o.previousJoltDictionary
									, o.minValue
								));
								return; //new not sure if it works
							}
							else { break; }
						}
						else if (o.depth < filteredButtons.Item1.Count - 1)
						{
							var newM = machine.SoftClone(false);
							newM.ApplyMultipleMove(i, filteredButtons.Item1[o.depth]);

							if (newM.joltageOverload(o.expectedJoltage, 0))
							{
								break;
							}
							else
							{
								list.Push((newM
									, o.score + i
									, o.usedJoltages.ToList()
									, o.buttonPressedForJoltage + i
									, o.depth + 1
									, o.expectedJoltage
									, o.Buttons
									, o.previousJoltDictionary
									, o.minValue
								));
							}
						}
					}

				});
			} while (list.Count > 0);

			var result = minResult.Sum(x => x.Value.Get() * machines.First(m => m.Id == x.Key).GCD);
			return result;
		}
	}
}