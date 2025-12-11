using AdventOfCode2025;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Program;
using System.Data;

[TestClass]
public class Day10Tests
{
	[TestMethod]
	[DataRow(true, 7)]
	//[DataRow(false, 419)]
	public void Day10_1(bool isExample, int expected)
	{
		var puzzle = new Day10(){ ReadSaveResult = false};
		;
		Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle),isExample));
	}

	[TestMethod]
	[DataRow(true, 33)]
	//[DataRow(false, 0)]
	public void Day10_2(bool isExample, long expected)
	{
		var puzzle = new Day10() { ReadSaveResult = false};
		Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle),isExample));
	}


	[TestMethod]
	public void Day10_Clone()
	{
		var puzzle = new Day10();
		var machine = puzzle.ParseInput(["[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}"], false)[0];
		var machineClone = machine.Clone();

		if (machineClone.Indicators.Count != machine.Indicators.Count)
		{
			throw new Exception();
		}
		for (int i = 0; i < machine.Indicators.Count;i++)
		{
			if(machineClone.Indicators[i] != machine.Indicators[i])
			{
				throw new Exception();
			}
		}
		machine.Indicators.Add(true);
		if (machineClone.Indicators.Count == machine.Indicators.Count)
		{
			throw new Exception();
		}

	}
	[TestMethod]
	//[DataRow("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}",2)]
	//[DataRow("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 3)]
	[DataRow("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 2)]
	public void ShouldGetCorrectNumber(string input, int count)
	{
		var puzzle = new Day10();
		Assert.AreEqual(count, puzzle.First([input],true));
	}
	[TestMethod]
	public void ManuallyApllyMove()
	{
		/*
	[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
	The fewest presses required to correctly configure it is 2; one way to do this is by pressing buttons (0,3,4) and (0,1,2,4,5) once each.*/
		var machine = new Day10().ParseInput(["[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}"],false)[0];
		machine.ApplyMove(1);
		Assert.IsFalse(machine.IsComplete());
		machine.ApplyMove(2);
		Assert.IsTrue(machine.IsComplete());

	}
	[TestMethod]
	public void Day10_TestApplyMove()
	{
		/*
		 You could press the first three buttons once each, a total of 3 button presses.
		You could press (1,3) once, (2,3) once, and (0,1) twice, a total of 4 button presses.
		 */

		var puzzle = new Day10();
		var machine = puzzle.ParseInput(["[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}"], false)[0];
		Assert.IsFalse(machine.IsComplete());
		machine.ApplyMove(0);
		Assert.IsFalse(machine.IsComplete());
		machine.ApplyMove(1);
		Assert.IsFalse(machine.IsComplete());
		machine.ApplyMove(2);
		Assert.IsTrue(machine.IsComplete());

		var m2 = puzzle.ParseInput(["[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}"],false)[0];
		Assert.IsFalse(m2.IsComplete());
		m2.ApplyMove(1);
		Assert.IsFalse(m2.IsComplete());
		m2.ApplyMove(3);
		Assert.IsTrue(m2.IsComplete());
		m2.ApplyMove(5);
		Assert.IsFalse(m2.IsComplete());
		m2.ApplyMove(5);
		Assert.IsTrue(m2.IsComplete());

		//The second machine can be configured with as few as 3 button presses:
		//One way to achieve this is by pressing the last three buttons ((0,4), (0,1,2), and (1,2,3,4)) once each.
		var m3 = puzzle.ParseInput(["[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}"],false)[0];
		Assert.IsFalse(m3.IsComplete());
		m3.ApplyMove(2);
		Assert.IsFalse(m3.IsComplete());
		m3.ApplyMove(3);
		Assert.IsFalse(m3.IsComplete());
		m3.ApplyMove(4);
		Assert.IsTrue(m3.IsComplete());
	}

	[TestMethod]
	[DataRow("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 10)]
	[DataRow("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 12)]
	[DataRow("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 11)]
	//[DataRow("[#.#####] (2,3,4,6) (2,5) (1,3,4,5,6) (1,2,5,6) (0,5,6) (0,1,2,3,4,6) (1,2,3,5,6) (1,3,4,6) (0,2,3,4,5,6) {23,42,62,53,35,62,74}", 74)]
	//[DataRow("[..#....#..] (0,4,5,7,8) (7,8) (1,3,5,6,9) (0,2,3,4,7,8,9) (1,4,6,7) (0,1,4,6,7,8,9) (3,4,7,9) (0,2,3,4,5,6,8,9) (2,5,7,9) (1,6,7,8,9) (0,2,3,5,6,7,8,9) (0,1,3,5,6,8,9) (5,8,9) {248,48,228,265,250,83,75,269,260,287}", 11)]
	public void Day10_2_Test(string input, int count)
	{
		var puzzle = new Day10();
		Assert.AreEqual(count, puzzle.Second([input],true));
	}

	[TestMethod]
	public void PriorityQueue()
	{
		var q = new PriorityQueue<string, int>();
		q.Enqueue("låg prio", 0);
		q.Enqueue("högprio", 100);
		var b = q.Dequeue();

	}
}