using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode2025
{
	public class Machine
	{
		public int Id { get; set; }
		public Machine() : this(true)
		{

		}
		public Machine(bool initiateFull)
		{
			if(initiateFull)
			{

				this.Buttons = new List<List<int>>();
				this.ExpectedIndicators = new List<bool>();
				this.ExpectedJoltage = new List<int>();
				this.PreviousMoves = new List<int>();
				this.PreviousMoveIndex = new List<int>();
				this.ButtonsBiggestValue = new List<int>();
			}
			this.Indicators = new List<bool>();
			this.joltage = new List<int>();
		}
		public List<bool> Indicators{ get; }
		public List<bool> ExpectedIndicators{ get; }
		public List<List<int>>	Buttons{ get; set; }
		public List<int> ButtonsBiggestValue{ get; }
		public List<int> joltage{ get; }
		public List<int> ExpectedJoltage { get; }

		public bool joltageOverload(List<int> expectedJoltage)
		{
			for (int i = 0; i < this.Indicators.Count; i++)
			{
				if (joltage[i] > expectedJoltage[i])
				{
					return true;
				}
			}
			return false;
		}

		public bool joltageIsComplete(List<int> expectedJoltage)
		{
			return this.joltageIsComplete(expectedJoltage, expectedJoltage.Count-1);
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
		public List<int> PreviousMoves { get; set; }
		public List<int> PreviousMoveIndex { get; set; }
		public void ApplyMove(int buttonIndex) //selecting group (1,2,3) (0,3,5) etc
		{
			this.ApplyMove(buttonIndex, this.Buttons);
		}
		public void ApplyMove(int buttonIndex, List<List<int>> buttons) //selecting group (1,2,3) (0,3,5) etc
		{
			this.PreviousMoveIndex?.Add(buttonIndex);
			for(int i = 0;i < buttons[buttonIndex].Count;i++)
			{
				this.ApplyMoveInternal(buttons[buttonIndex][i]);
			}
		}
		private void ApplyMoveInternal(int move) 
		{
			this.PreviousMoves?.Add(move);
			this.Indicators[move] = !this.Indicators[move];
			this.joltage[move]++;
		}

		public static List<List<int>> FilterMoves(Dictionary<int, List<List<int>>> filteredMoves, List<List<int>> buttons , int joltIndex)
		{
			if(filteredMoves.ContainsKey(joltIndex))
			{
				return filteredMoves[joltIndex];
			}
			var moves = FilterMoves(joltIndex, buttons);
			filteredMoves.Add(joltIndex, moves);
			return moves;
		}
		private static List<List<int>> FilterMoves(int joltIndex, List<List<int>> buttons)
		{
			return buttons.Where(x => x.Any(y => y == joltIndex)).ToList();
		}

		public Machine Clone()
		{
			var clone = new Machine
			{
				Id = this.Id
			};
			clone.Indicators.AddRange(this.Indicators);
			clone.ExpectedIndicators.AddRange(this.ExpectedIndicators);
			foreach (var row in this.Buttons)
			{
				clone.Buttons.Add(new List<int>(row));
			}
			clone.joltage.AddRange(this.joltage);
			clone.ExpectedJoltage.AddRange(this.ExpectedJoltage);
			clone.PreviousMoves.AddRange(this.PreviousMoves);
			clone.PreviousMoveIndex.AddRange(this.PreviousMoveIndex);
			clone.ButtonsBiggestValue.AddRange(this.ButtonsBiggestValue);

			return clone;
		}
		public Machine SoftClone()
		{
			var clone = new Machine(false)
			{
				Id = this.Id
			};
			clone.Indicators.AddRange(this.Indicators);
			clone.joltage.AddRange(this.joltage);
			return clone;
		}
	}
	public class Day10
	{
		public bool ReadSaveResult { get; set; }

		public long First(IList<string> input, bool isExample)
		{
			//Change everything to binary numbers and calculte that way.
			var machines = this.ParseInput(input,true);
			var concurrentBag = new ConcurrentBag<int>();
			//foreach(var machine in machines) 
			Parallel.ForEach(machines, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 1) }, machine =>
			{
				var t = new Stopwatch();
				t.Start();
				var result = FileReader.GetResult(isExample,Part.One, this, machine.Id);
				if(ReadSaveResult && !string.IsNullOrEmpty(result))
				{
					var count = int.Parse(result);
					concurrentBag.Add(count);
					this.PrintMachineInfoAndTime(machine, count);
				}
				else
				{

					var count = this.SolveMachine(machine);
					t.Stop();
					concurrentBag.Add(count);
					this.PrintMachineInfoAndTime(t, machine, count);
					FileReader.SaveResult(isExample,Part.One, this, machine.Id, count.ToString());
				}
			}
			);

			return concurrentBag.Sum();
		}
		public void PrintMachineInfoAndTime(Stopwatch t, Machine machine, int result)
		{
			Console.WriteLine($"Id: {machine.Id}	Took: {t.Elapsed}	Count : {result}	ButtonPair: {machine.Buttons.Count()}	Buttons: {machine.Buttons.Sum(x => x.Count)}");
		}
		public void PrintMachineInfoAndTime(Machine machine, int result)
		{
			Console.WriteLine($"Id: {machine.Id}	Was Saved	Count : {result}	ButtonPair: {machine.Buttons.Count()}	Buttons: {machine.Buttons.Sum(x => x.Count)}");
		}
		public long Second(IList<string> input,bool isExample)
		{
			var random = new Random();
			var machines = this.ParseInput(input,true).OrderBy(x => random.Next(0,100));
			var concurrentBag = new ConcurrentBag<long>();

			//foreach (var machine in machines)
			Parallel.ForEach(machines
				, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount) }
				, machine =>
				{
					Console.WriteLine($"Started with: {machine.Id}");
					var t = new Stopwatch();
					t.Start();
					var result = FileReader.GetResult(isExample, Part.Two, this, machine.Id);
					if (ReadSaveResult && !string.IsNullOrEmpty(result))
					{
						var count = int.Parse(result);
						concurrentBag.Add(count);
						this.PrintMachineInfoAndTime(machine, count);
					}
					else
					{
						var count = this.SolveMachineSecond(machine);
						t.Stop();
						concurrentBag.Add(count);
						this.PrintMachineInfoAndTime(t, machine, count);
						if(ReadSaveResult)
						{
							FileReader.SaveResult(isExample, Part.Two, this, machine.Id, count.ToString());
						}
					}
					Console.WriteLine($"Done with {concurrentBag.Count()} / {input.Count}");
			}
			);

			return concurrentBag.Sum();
		}
		
		public int GetPrio(Machine machine, int buttonIndex, Part part)
		{
			return machine.joltage.Count - machine.ButtonsBiggestValue[buttonIndex];
		}
		public int SolveMachine(Machine machine)
		{
			var ecpectedIndicator = machine.ExpectedIndicators.ToList();
			var buttons = machine.Buttons.ToList();
			//var q = new PriorityQueue<(Machine m, IList<int> moves,int move), int >();
			var q = new Stack<(Machine m, IList<int> moves,int move)>();
			for (int i = 0; i < machine.Buttons.Count;i++)
			{
				var m = machine.SoftClone();
				q.Push((m, [], i));
				//q.Enqueue((m, [], i), GetPrio(m, i, part));
			}
			var minCount = int.MaxValue;
			while(q.Count > 0)
			{
				var l = q.Pop();
				//var l = q.Dequeue();
				l.m.ApplyMove(l.move,buttons);
				l.moves.Add(l.move);
				if (l.moves.Count>minCount
				
				){
					continue;
				}
				if( l.m.IsComplete(ecpectedIndicator))
				{
					if (l.moves.Count < minCount)
					{
						minCount = l.moves.Count;
						continue;
					}
				}


				var moveIndex = new List<int>();
				for(int m = 0; m < buttons.Count;m++)
				{
					if(!l.moves.Contains(m)
						//&& buttons.Any(x => l.m.ExpectedIndicators[x])
						)
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



		public int SolveMachineSecond(Machine machine)
		{
			var originalButtons = machine.Buttons.ToList();
			var expectedJoltage = machine.ExpectedJoltage.ToList();
			var filteredMoves = new Dictionary<int, List<List<int>>>();
			var filteredButtons = Machine.FilterMoves(filteredMoves, originalButtons, 0).ToList();
			var q = new Stack<(Machine m, int count, int move, int joltPos)>();
			for(int i = 0; i < machine.Buttons.Count;i++)
			{
				if(Machine.FilterMoves(filteredMoves, originalButtons, 0).ToList().Contains(machine.Buttons[i]))
				{
					q.Push((machine.SoftClone(), 1, i, 0));
				}
			}
			var hash = new Dictionary<int, int>();
			for(int i=0;i < expectedJoltage.Count; i++)
			{
				hash.Add(i, int.MaxValue);
			}
			var minCount = int.MaxValue;
			


			while (q.Count > 0)
			{
					
				var l = q.Pop();
				l.m.ApplyMove(l.move,originalButtons);

				if (l.count > minCount || l.m.joltageOverload(expectedJoltage) // pushed too many times on a button
				)
				{
					continue;
				}
				else if (l.count < minCount && l.m.joltageIsComplete(expectedJoltage)) //pushed valid amount of times
				{
					 //found a better combo
					minCount = l.count;
					continue;
				}
				else if (l.m.joltageIsComplete(expectedJoltage, l.joltPos) //first x amount of jolts are valid
					&& l.m.joltage.Count > l.joltPos
					)
				{
					hash[l.joltPos] = l.count < hash[l.joltPos] 
						? l.count
						: hash[l.joltPos]
						;
					var joltPosition = l.joltPos + 1;
					while( joltPosition < expectedJoltage.Count-1 && l.m.joltageIsComplete(expectedJoltage, joltPosition))
					{
						joltPosition++;
					}
					var joltMoves1 = Machine.FilterMoves(filteredMoves, originalButtons, joltPosition);

					for (int i = 0; i < originalButtons.Count; i++)
					{
						if (joltMoves1.Contains(originalButtons[i]))
						{
							q.Push((l.m.SoftClone(), l.count + 1, i, joltPosition));
						}
					}
				}
				else
				{
					var joltMoves = Machine.FilterMoves(filteredMoves, originalButtons, l.joltPos);
					for (int i = 0; i < originalButtons.Count; i++)
					{
						if (joltMoves.Contains(originalButtons[i]))
						{
							q.Push((l.m.SoftClone(), l.count + 1, i, l.joltPos));
						}
					}

				}
			}
			return minCount;
		}
		public List<Machine> ParseInput(IList<string> input, bool sortButtons)
		{
			var result = new List<Machine>();
			var id = 0;
			foreach (var item in input) 
			{
				var machine = new Machine() { Id = id++ };
				var indicatorString = new string(item.TakeWhile(x => x != ']').ToArray());
				for(int i = 1; i<indicatorString.Length; i++)
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
				foreach(var buttons in buttonPairs)
				{
					var b = new List<int>();
					foreach (var button in buttons.Split(','))
					{
						b.Add(int.Parse(button));
					}
					machine.Buttons.Add(b.OrderBy(x => x).ToList());
					if(sortButtons)
					{

						//machine.Buttons = machine.Buttons.OrderBy(x => x.Min()).ToList();
					}
					
					machine.ButtonsBiggestValue.Add(b.Max());
				}
				var joltage = item[indicatorString.Length..].Split('{')[1].Replace("}", "");
				var jolts = new List<int>();
				foreach(var jolt in joltage.Split(','))
				{
					jolts.Add(int.Parse(jolt));
					//machine.ExpectedJoltage.Add(int.Parse(jolt));
					machine.joltage.Add(0);//start value
				}

				var gcd = FindGCD(jolts);
				foreach(var jolt in jolts)
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
