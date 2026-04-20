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

		private static long _firstTick = 0;

		public static bool Check()
		{
			long now = DateTime.UtcNow.Ticks;

			// First call: store the tick
			if (_firstTick == 0)
			{
				_firstTick = now;
				return true;
			}

			// Subsequent calls: compare
			long elapsed = now - _firstTick;

			return elapsed < TimeSpan.TicksPerSecond * 1;
		}

		public long SolveWithConcurrentList(IList<string> input)
		{
			var machines = this.day10.ParseInput(input).ToList();
			var minResult = new ConcurrentDictionary<int, SingleValueHolder>();
			var movesDictionary = new ConcurrentDictionary<int, ConcurrentDictionary<int, List<int[]>>>();

			var list = new ConcurrentStack<(Machine m
			, int score
			, List<int> usedJoltages
			, int buttonPressedForJoltage
			, int depth
			, List<int> expectedJoltage
			, List<List<int>> Buttons
			, ConcurrentDictionary<string, int> previousJoltDictionary
			)>();

			foreach (var m in machines)
			{
				list.Push((m, 0, new List<int>(), 0, 0, m.ExpectedJoltage, m.Buttons, new ConcurrentDictionary<string, int>()));
				minResult.TryAdd(m.Id, new SingleValueHolder(int.MaxValue));
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
					var minValue = minResult[machine.Id];
					if (o.score > minValue.Get())
					{
						return;
					}

					var filteredButtons = Machine.FilterMovesExcludePreviousJoltagesMovesThreadSafe(movesDictionary[o.m.Id]
						, o.Buttons
						, o.usedJoltages
						, o.previousJoltDictionary //Add this later for cache of moves
						//, null
						);
					if (filteredButtons.joltage == -1)
					{
						return;
					}
					var value = o.expectedJoltage[filteredButtons.joltage] - machine.joltage[filteredButtons.joltage];
					for (int i = 0; i <= o.expectedJoltage[filteredButtons.joltage] - o.buttonPressedForJoltage; i++)
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

							if (!joltageOverloaded)
							{
								list.Push((newMachene
									, newButtonPressedForJoltage
									, [filteredButtons.joltage, .. o.usedJoltages]
									, 0
									, 0
									, o.expectedJoltage
									, o.Buttons
									, o.previousJoltDictionary
								));
							}
							if (!joltageOverloaded && newMachene.joltageIsComplete(o.expectedJoltage, o.expectedJoltage.Count))
							{
								//Machene is done, look at result and update if better;
								if (o.score + i < minValue.Get())
								{
									//här kan minsta värdet råka skrivas över
									minValue.TryLower(o.score + i);

								}
								return;
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
									, [.. o.usedJoltages]
									, o.buttonPressedForJoltage + i
									, o.depth + 1
									, o.expectedJoltage
									, o.Buttons
									, o.previousJoltDictionary
								));
							}
						}
						else if (o.depth == filteredButtons.Item1.Count)
						{
							i = 1000;
						}
					}

				});
			} while (list.Count > 0);

			var result = minResult.Sum(x => x.Value.Get() * machines.First(m => m.Id == x.Key).GCD);
			return result;
		}
	}
}