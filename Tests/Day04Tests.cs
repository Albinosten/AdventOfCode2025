using AdventOfCode2025;

[TestClass]
public class Day04Tests
{   
    [TestMethod]
    [DataRow(true, 13)]
    [DataRow(false, 1478)]
    public void Day04_1(bool isExample, long expected)
    {
        var puzzle = new Day04();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }
    [TestMethod]
    [DataRow(true, 43)]
    [DataRow(false, 9120)]
    public void Day04_2(bool isExample, long expected)
    {
        var puzzle = new Day04();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }

}