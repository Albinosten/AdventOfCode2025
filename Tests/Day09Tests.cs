using AdventOfCode2025;

[TestClass]
public class Day09Tests
{
    [TestMethod]
    [DataRow(true, 50)]
    [DataRow(false, 4777967538)]
    public void Day09_1(bool isExample, long expected)
    {
        var puzzle = new Day09();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }

    [TestMethod]
    [DataRow(true, 24)]
    [DataRow(false, 1439894345)]
    public void Day09_2(bool isExample, long expected)
    {
        var puzzle = new Day09();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }
}