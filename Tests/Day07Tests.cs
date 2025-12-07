using AdventOfCode2025;

[TestClass]
public class Day07Tests
{
    [TestMethod]
    [DataRow(true, 21)]
    [DataRow(false, 1560)]
    public void Day07_1(bool isExample, int expected)
    {
        var puzzle = new Day07();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }
    [TestMethod]
    [DataRow(true, 40)]
    [DataRow(false, 25592971184998)]
    public void Day07_2(bool isExample, long expected)
    {
        var puzzle = new Day07();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }
}