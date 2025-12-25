using System.Collections.Concurrent;
using System.Diagnostics;

namespace AdventOfCode2025
{
	public class Machine
	{
		public int Id { get; set; }
		public int GCD { get; set; }
		public string Input{ get; set; }
		public Machine(int joltageCount) : this(true, joltageCount)
		{
		}
		public Machine(bool initiateFull, int joltageCount)
		{
			if (initiateFull)
			{

				this.Buttons = new List<List<int>>();
				this.ExpectedIndicators = new List<bool>();
				this.ExpectedJoltage = new List<int>();
			}

			this.Indicators = new List<bool>(joltageCount);
			this.joltage = new int[joltageCount];

		}
		public List<bool> Indicators { get; }
		public List<bool> ExpectedIndicators { get; }
		public List<List<int>> Buttons { get; set; }
		
		public int[] joltage { get; }
		public List<int> ExpectedJoltage { get; }

		public bool joltageOverload(int[] expectedJoltage, int startIndex)
		{
			for (int i = startIndex; i < expectedJoltage.Length; i++)
			{
				if (joltage[i] > expectedJoltage[i])
				{
					return true;
				}
			}
			return false;
		}
		public bool joltageOverloadBackwards(int[] expectedJoltage, int startIndex) //Could use startindex here but for now its expectedJoltage.Length
		{
			for (int i = expectedJoltage.Length-1; i >= 0; i--)
			{
				if (joltage[i] > expectedJoltage[i])
				{
					return true;
				}
			}
			return false;
		}


		public bool joltageIsComplete(List<int> expectedJoltage, int joltIndex)
		{
			for (int i = 0; i <= joltIndex; i++)
			{
				if (joltage[i] != expectedJoltage[i])
				{
					return false;
				}
			}
			return true;
		}
		public bool joltageIsComplete(int[] expectedJoltage, int joltIndex)
		{
			for (int i = 0; i < joltIndex; i++)
			{
				if (joltage[i] != expectedJoltage[i])
				{
					return false;
				}
			}
			return true;
		}
		public bool IsComplete()
		{
			return this.IsComplete(this.ExpectedIndicators);
		}
		public bool IsComplete(IList<bool> expectedIndicators)
		{
			return this.IsComplete(expectedIndicators, expectedIndicators.Count);
		}
		public bool IsComplete(IList<bool> expectedIndicators, int index)
		{
			for (int i = 0; i < index; i++)
			{
				if (Indicators[i] != expectedIndicators[i])
				{
					return false;
				}
			}
			return true;
		}
		public void ApplyMove(int buttonIndex) //selecting group (1,2,3) (0,3,5) etc
		{
			this.ApplyMove(buttonIndex, this.Buttons);
		}
		public void ApplyMove(int buttonIndex, List<List<int>> buttons) //selecting group (1,2,3) (0,3,5) etc
		{
			for (int i = 0; i < buttons[buttonIndex].Count; i++)
			{
				this.ApplyMoveInternal(buttons[buttonIndex][i]);
			}
		}
		private void ApplyMoveInternal(int move)
		{
			this.Indicators[move] = !this.Indicators[move];
			this.joltage[move]++;
		}
		public void ApplyMultipleMove(int count, int[] buttons)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				this.joltage[buttons[i]] += count;
			}
		}
		public static List<int[]> FilterMovesExcludePreviousJoltageMoves(Dictionary<int, List<int[]>> movesDictionary, List<List<int>> allButtons, int joltIndex)
		{
			if (movesDictionary.ContainsKey(joltIndex))
			{
				return movesDictionary[joltIndex];
			}
			var moves = allButtons
				.Where(x => x.Any(y => y == joltIndex))
				.Where(x => x.All(y => y >= joltIndex))
				.OrderByDescending(x => x.Count)
				.Select(x => x.ToArray())
				.ToList();
			movesDictionary.Add(joltIndex, moves);
			return moves;
		}
		public static List<int[]> FilterMovesExcludePreviousJoltageMovesBackwards(Dictionary<int, List<int[]>> movesDictionary, List<List<int>> allButtons, int joltIndex)
		{
			if (movesDictionary.ContainsKey(joltIndex))
			{
				return movesDictionary[joltIndex];
			}
			var moves = allButtons
				.Where(x => x.Any(y => y == joltIndex))
				.Where(x => x.All(y => y <= joltIndex))
				.OrderByDescending(x => x.Count)
				.Select(x => x.ToArray())
				.ToList();
			movesDictionary.Add(joltIndex, moves);
			return moves;
		}

		public static string GetHash(List<int> ints)
		{
			return string.Join(",", ints
				// .Distinct()
				// .OrderBy(x => x)
				// .ToList()
				);
		}
		public static (List<int[]> buttons, int joltage) FilterMovesExcludePreviousJoltagesMoves(
			Dictionary<int, List<int[]>> movesDictionary
		, List<List<int>> allButtons
		, List<int> usedJoltage
		, Dictionary<string, int> nextJoltage
		)
		{
			var joltHash = GetHash(usedJoltage);
			if (nextJoltage.TryGetValue(joltHash, out var nj) 
				&& movesDictionary.ContainsKey(nj))
			{
				return (movesDictionary[nj], nj);
			}
			
				
			//(0,1,2) (1,2) (2,3)
			var unusedButtons = allButtons.
				Where(x => !x.Any(c => usedJoltage.Contains(c)))
				.ToList();

			var joltIndex = unusedButtons
				.SelectMany(x => x)
				.GroupBy(x => x)
				.OrderBy(x => x.Count())
				.FirstOrDefault()?.Key 
				?? -1
				;
			
			var moves = unusedButtons
				.Where(x => x.Any(y => y == joltIndex))
				.OrderByDescending(x => x.Count)
				.Select(x => x.ToArray())
				.ToList();
			movesDictionary.Add(joltIndex, moves);
			nextJoltage.Add(joltHash, joltIndex);
			return (moves,joltIndex);
			
		}
		public Machine Clone()
		{
			var clone = new Machine(this.joltage.Length)
			{
				Id = this.Id,
				GCD = this.GCD,
			};
			foreach (var row in this.Buttons)
			{
				clone.Buttons.Add(new List<int>(row));
			}
			Array.Copy(this.joltage, clone.joltage, this.joltage.Length);
			clone.Indicators.AddRange(this.Indicators);
			clone.ExpectedIndicators.AddRange(this.ExpectedIndicators);
			clone.ExpectedJoltage.AddRange(this.ExpectedJoltage);

			return clone;
		}
		public Machine SoftClone(bool cloneIndicator = true)
		{
			var clone = new Machine(false, this.joltage.Length)
			{
				Id = this.Id,
				GCD = this.GCD,
			};
			if (cloneIndicator)
			{
				for (int i = 0; i < this.Indicators.Count; i++)
				{
					clone.Indicators.Add(this.Indicators[i]);
				}
			}
			Array.Copy(this.joltage, clone.joltage, this.joltage.Length);

			return clone;
		}
	}
	public class Day10
	{
		public bool ReadSaveResult { get; set; }

		public long First(IList<string> input, bool isExample)
		{
			var machines = this.ParseInput(input);
			var concurrentBag = new ConcurrentBag<int>();
			Parallel.ForEach(machines
			, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 1) }
			, machine =>
			{
				var t = new Stopwatch();
				t.Start();
				var result = FileReader.GetResult(isExample, Part.One, this, machine.Id);
				if (ReadSaveResult && !string.IsNullOrEmpty(result))
				{
					var count = int.Parse(result);
					concurrentBag.Add(count);
				}
				else
				{
					var count = this.SolveMachine(machine);
					t.Stop();
					concurrentBag.Add(count);
					this.PrintMachineInfoAndTime(t, machine, count);
					if(ReadSaveResult)
					{
						FileReader.SaveResult(isExample, Part.One, this, machine.Id, count.ToString());
					}
				}
			});

			return concurrentBag.Sum();
		}
		public void PrintMachineInfoAndTime(Stopwatch t, Machine machine, int result)
		{
			Console.WriteLine($"Id: {machine.Id}	Took: {t.Elapsed}	Count : {result}	ButtonPair: {machine.Buttons.Count()}	Buttons: {machine.Buttons.Sum(x => x.Count)}");
		}
		public long Second(IList<string> input, bool isExample)
		{
			List<Machine> machines = this.ParseInput(input)
				.ToList()
				;
			var concurrentBag = new ConcurrentBag<long>();
			// machines.Reverse();
			// foreach(var machine in machines)
			Parallel.ForEach(machines
			, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount) }
			, machine =>
			{
				var t = new Stopwatch();
				t.Start();
				var result = FileReader.GetResult(isExample, Part.Two, this, machine.Id);
				if (ReadSaveResult && !string.IsNullOrEmpty(result))
				{
					var count = int.Parse(result);
					Console.WriteLine($"ID: {machine.Id} result: {count}");
					concurrentBag.Add(count);
				}
				else
				{
					Console.WriteLine($"Started with: {machine.Id}");
					var count = (int)this.RunBruitForceRecursiveMinJoltage(machine, new SingleValueHolder(int.MaxValue), new CancellationTokenSource());
					t.Stop();
					concurrentBag.Add(count);
					this.PrintMachineInfoAndTime(t, machine, count);
					Console.WriteLine($"Done with {concurrentBag.Count()} / {input.Count} At: {DateTime.Now}");
					
					Console.WriteLine("*******************");
					if (ReadSaveResult)
					{
						FileReader.SaveResult(isExample, Part.Two, this, machine.Id, count.ToString());
					}
				}
			}
			);

			return concurrentBag.Sum();
		}

		
		public long RunBruitForceRecursiveMinJoltage(Machine machine, SingleValueHolder svh, CancellationTokenSource cancellationToken)
		{
			var movesDictionary = new Dictionary<int, List<int[]>>();
			int minScore = int.MaxValue;
			return this.BruitForceRecursiveMinJoltage(machine
			, 0
			, 0
			, 0
			, machine.Buttons
			, new List<int>()
			, movesDictionary
			, new Dictionary<string, int>()
			, ref minScore
			, machine.ExpectedJoltage.ToArray()
			, cancellationToken
			)
			* machine.GCD
			;
		}

		public int BruitForceRecursiveMinJoltage(Machine m
		, int buttonPressedForJoltage
		, int score
		, int depth
		, List<List<int>> originalButtons
		, List<int> usedJoltages
		, Dictionary<int, List<int[]>> movesDictionary
		, Dictionary<string, int> previousJoltDictionary
		// , SingleValueHolder svh
		, ref int minScore
		, int[] expectedJoltage
		, CancellationTokenSource cancellationToken
		)
		{
			cancellationToken.Token.ThrowIfCancellationRequested();
			// var minScore = svh.Get();
			if (score > minScore)
			{
				return score;
			}
			var filteredButtons = Machine.FilterMovesExcludePreviousJoltagesMoves(movesDictionary
				, originalButtons
				, usedJoltages
				, previousJoltDictionary
				);
			if(filteredButtons.joltage == -1)
			{
				return score;
			}
			var value = expectedJoltage[filteredButtons.joltage] - m.joltage[filteredButtons.joltage];
			for (int i = 0; i <= expectedJoltage[filteredButtons.joltage] - buttonPressedForJoltage; i++)
			{
				if (i == value)
				{
					var newM = m.SoftClone(false);
	
					var newButtonPressedForJoltage = score;
					if (filteredButtons.Item1.Count > 0)
					{
						newButtonPressedForJoltage += i;
						newM.ApplyMultipleMove(i, filteredButtons.Item1[depth]);
					}

					/*sending joltage-1 here since the forloop makes sure to not overdo current joltage*/
					/*And FilterMovesExcludePreviousJoltageMoves makes sure no previous joltages are changed*/
					var joltageOverloaded = newM.joltageOverload(expectedJoltage, 0);
					if (!joltageOverloaded)
					{
						if (BruitForceRecursiveMinJoltage(newM.SoftClone(false), 0
							, newButtonPressedForJoltage
							, 0
							, originalButtons
							, [filteredButtons.joltage, ..usedJoltages]
							, movesDictionary
							, previousJoltDictionary
							, ref minScore
							, expectedJoltage
							, cancellationToken
							) >= minScore)
						{
							break;
						}
					}
					if (!joltageOverloaded && newM.joltageIsComplete(expectedJoltage, expectedJoltage.Length))
					{
						minScore = score + i;
						// svh.Set(minScore);
						return minScore;
					}
					else { break; }
				}
				else if (depth < filteredButtons.Item1.Count - 1)
				{
					var newM = m.SoftClone(false);
					newM.ApplyMultipleMove(i, filteredButtons.Item1[depth]);

					if (newM.joltageOverload(expectedJoltage, 0)
						|| BruitForceRecursiveMinJoltage(newM, buttonPressedForJoltage + i
							, score + i
							, depth + 1
							, originalButtons
							, [..usedJoltages]
							, movesDictionary
							, previousJoltDictionary
							, ref minScore
							, expectedJoltage
							, cancellationToken
							) > minScore)
					{
						break;
					}
				}
				else if (depth == filteredButtons.Item1.Count)
				{
					i = 1000;
				}
			}
			return minScore;
		}

		public int SolveMachine(Machine machine)
		{
			var ecpectedIndicator = machine.ExpectedIndicators.ToList();
			var buttons = machine.Buttons.ToList();
			var q = new Stack<(Machine m, IList<int> moves, int move)>();
			for (int i = 0; i < machine.Buttons.Count; i++)
			{
				var m = machine.SoftClone();
				q.Push((m, [], i));
			}
			var minCount = int.MaxValue;
			var minResult = new List<int>();
			var result = new Dictionary<int, List<List<int>>>();
			while (q.Count > 0)
			{
				var l = q.Pop();
				l.m.ApplyMove(l.move, buttons);
				l.moves.Add(l.move);
				if (l.moves.Count > minCount)
				{
					continue;
				}
				if (l.m.IsComplete(ecpectedIndicator))
				{
					if (l.moves.Count <= minCount)
					{
						minCount = l.moves.Count;
						minResult = l.moves.ToList();
						if(result.ContainsKey(l.moves.Count))
						{
							result[l.moves.Count].Add(l.moves.ToList());
						}
						else
						{
							var newList = new List<List<int>>
							{
								l.moves.ToList()
							};
							result.Add(l.moves.Count, newList);
						}
						continue;
					}
				}


				var moveIndex = new List<int>();
				for (int m = 0; m < buttons.Count; m++)
				{
					if (!l.moves.Contains(m))
					{
						moveIndex.Add(m);
					}

				}

				foreach (var m in moveIndex)
				{
					var mC = l.m.SoftClone();
					q.Push((mC, l.moves.ToList(), m));
				}
			}

			return minCount;
		}

		public bool VerifyNewFunctionWithSavedResult(IList<string> input, int count)
		{
			var machines = this.ParseInput(input)
				.Take(count);
				;
			foreach (var machine in machines)
			{
				var resultFromfile = FileReader.GetResult(false, Part.Two, this, machine.Id);
				if (!string.IsNullOrEmpty(resultFromfile))
				{
					Console.WriteLine("Id: " + machine.Id);
					var savedResult = int.Parse(resultFromfile);
					var t = new Stopwatch();
					t.Start();
					var newResult = this.RunBruitForceRecursiveMinJoltage(machine, new SingleValueHolder(int.MaxValue), new CancellationTokenSource());
					
					if (newResult != savedResult)
					{
						throw new Exception();
					}

				}
			}
			return true;
		}

		public List<Machine> ParseInput(IList<string> input)
		{
			var result = new List<Machine>();
			var id = 0;
			foreach (var item in input)
			{
				var indicatorString = new string(item.TakeWhile(x => x != ']').ToArray());
				var machine = new Machine(indicatorString.Count()-1) { Id = id++ };
				machine.Input = item;
				for (int i = 1; i < indicatorString.Length; i++)
				{
					var v = indicatorString[i] == '.' ? false : true;
					machine.ExpectedIndicators.Add(v);
					machine.Indicators.Add(false);//Start values
				}


				var buttonPairs = new string(item[indicatorString.Length..]
					.Split('{')[0]
					.Replace(']', ')')
					+ '(')
					.Split(") (")
					.Where(x => !string.IsNullOrEmpty(x));
				foreach (var buttons in buttonPairs)
				{
					var b = new List<int>();
					foreach (var button in buttons.Split(','))
					{
						b.Add(int.Parse(button));
					}
					machine.Buttons.Add(b.OrderBy(x => x).ToList());
				}
				var joltage = item[indicatorString.Length..].Split('{')[1].Replace("}", "");
				var jolts = new List<int>();
				var joltageSplit = joltage.Split(',');
				for(int i = 0; i < joltageSplit.Count(); i++) 
				{
					jolts.Add(int.Parse(joltageSplit[i]));
					machine.joltage[i]= 0;//start value
				}

				var gcd = FindGCD(jolts);
				machine.GCD = gcd;
				foreach (var jolt in jolts)
				{
					machine.ExpectedJoltage.Add(jolt / gcd);
				}

				result.Add(machine);
			}
			return result;
		}
		public static int GCD(int a, int b)
		{
			if (a == 0)
			{
				return b;
			}
			return GCD(b % a, a);
		}

		public static int FindGCD(IList<int> array)
		{
			int res = array[0];
			for (int i = 1; i < array.Count; i++)
			{
				res = GCD(array[i], res);
				if (res == 1)
					return 1;
			}
			return res;
		}
	}
}
