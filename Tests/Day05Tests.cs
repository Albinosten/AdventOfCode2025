using AdventOfCode2025;

[TestClass]
public class Day05Tests
{   
    [TestMethod]
    [DataRow(true, 3)]
    [DataRow(false, 525)]
    public void Day05_1(bool isExample, long expected)
    {
        var puzzle = new Day05();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }
    [TestMethod]
    [DataRow(true, 14)]
    [DataRow(false, 333892124923577)]
    public void Day05_2(bool isExample, long expected)
    {
        var puzzle = new Day05();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }

}