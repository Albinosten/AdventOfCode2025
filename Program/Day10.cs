using System.Collections.Concurrent;
using System.Diagnostics;

namespace AdventOfCode2025
{
	public class Machine
	{
		public int Id { get; set; }
		public Machine()
		{
			this.Buttons = new List<List<int>>();
			this.Indicators = new List<bool>();
			this.ExpectedIndicators = new List<bool>();
			this.joltage = new List<int>();
			this.ExpectedJoltage = new List<int>();
			this.PreviousMoves = new List<int>();
			this.PreviousMoveIndex = new List<int>();
			this.ButtonsBiggestValue = new List<int>();
		}
		public List<bool> Indicators{ get; }
		public List<bool> ExpectedIndicators{ get; }
		public List<List<int>>	Buttons{ get; }
		public List<int> ButtonsBiggestValue{ get; }
		public List<int> joltage{ get; }
		public List<int> ExpectedJoltage { get; }

		public bool joltageOverload()
		{
			for (int i = 0; i < this.Indicators.Count; i++)
			{
				if (joltage[i] > ExpectedJoltage[i])
				{
					return true;
				}
			}
			return false;
		}
		public bool joltageIsComplete()
		{
			for (int i = 0; i < this.Indicators.Count; i++)
			{
				if (joltage[i] != ExpectedJoltage[i])
				{
					return false;
				}
			}
			return true;
		}
		public bool IsComplete()
		{
			for(int i = 0;  i < this.Indicators.Count; i++)
			{
				if(Indicators[i] != ExpectedIndicators[i])
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
			this.PreviousMoveIndex.Add(buttonIndex);
			foreach(var move in Buttons[buttonIndex])
			{
				//applying the move (1,2,3). 1,2 and 3
				this.ApplyMoveInternal(move);
			}
		}
		private void ApplyMoveInternal(int move) 
		{
			this.PreviousMoves.Add(move);
			this.Indicators[move] = !this.Indicators[move];
			this.joltage[move]++;
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
	}
	public class Day10
	{

		public long First(IList<string> input)
		{
			//Change everything to binary numbers and calculte that way.
			var machines = this.ParseInput(input);
			var concurrentBag = new ConcurrentBag<int>();
			//foreach(var machine in machines) 
			Parallel.ForEach(machines, machine =>
			{
				var t = new Stopwatch();
				t.Start();
				var count = this.SolveMachine(machine, Part.One);
				t.Stop();
				concurrentBag.Add(count);
				var text = $"Took: {t.Elapsed}	Count : {count}	ButtonPair: {machine.Buttons.Count()}	Buttons: {machine.Buttons.Sum(x => x.Count)}";
				Console.WriteLine(text);
			}
			);

			return concurrentBag.Sum();
		}
		public long Second(IList<string> input)
		{
			var machines = this.ParseInput(input);
			var concurrentBag = new ConcurrentBag<long>();
			//foreach(var machine in machines) 
			Parallel.ForEach(machines, machine =>
			{
				var t = new Stopwatch();
				t.Start();
				var count = this.SolveMachine(machine, Part.Two);
				t.Stop();
				concurrentBag.Add(count);
				var text = $"Took: {t.Elapsed}	Count : {count}	ButtonPair: {machine.Buttons.Count()}	Buttons: {machine.Buttons.Sum(x => x.Count)}";
				Console.WriteLine(text);
			}
			);

			return concurrentBag.Sum();
		}
		public enum Part { One,Two}
		public int GetPrio(Machine machine, int buttonIndex, Part part)
		{
			return machine.joltage.Count - machine.ButtonsBiggestValue[buttonIndex];
		}
		public int SolveMachine(Machine machine, Part part)
		{
			//var q = new PriorityQueue<(Machine m, IList<int> moves,int move), int >();
			var q = new Stack<(Machine m, IList<int> moves,int move)>();
			for (int i = 0; i < machine.Buttons.Count;i++)
			{
				var m = machine.Clone();
				q.Push((m, [], i));
				//q.Enqueue((m, [], i), GetPrio(m, i, part));
			}
			var minCount = int.MaxValue;
			while(q.Count > 0)
			{
				var l = q.Pop();
				//var l = q.Dequeue();
				l.m.ApplyMove(l.move);
				l.moves.Add(l.move);
				if (l.moves.Count>minCount
				|| part == Part.Two && l.m.joltageOverload()
				){
					continue;
				}
				if(part == Part.One && l.m.IsComplete()
					|| part == Part.Two && l.m.joltageIsComplete()
				)
				{
					if (l.moves.Count < minCount)
					{
						minCount = l.moves.Count;
						continue;
					}
				}


				var moveIndex = new List<int>();
				for(int m = 0; m < l.m.Buttons.Count;m++)
				{
					var buttons = l.m.Buttons[m];
					if(part == Part.One 
						&& !l.moves.Contains(m)
						//&& buttons.Any(x => l.m.ExpectedIndicators[x])
						)
					{
						moveIndex.Add(m);
					}
					else if(part == Part.Two)
					{
						moveIndex.Add(m);
					}
				}

				if(part == Part.Two)
				{
				//start with button group that have the highest internal number
				//if jolt.Count == 12 then take a button group with a number 12 in it (x,y, 12, z)
					
					moveIndex = moveIndex
						.OrderByDescending(x => GetPrio(l.m, x, part))
						.ToList();
				}

				foreach (var m in moveIndex)
				{
					var mC = l.m.Clone();
					//q.Enqueue((mC, l.moves.ToList(), m),GetPrio(mC,m,part));
					q.Push((mC, l.moves.ToList(), m));
				}
			}

			return minCount;
		}
		public List<Machine> ParseInput(IList<string> input)
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
					machine.Buttons.Add(b);
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
