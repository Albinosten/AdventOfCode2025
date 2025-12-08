using AdventOfCode2025;

[TestClass]
public class Day08Tests
{
    [TestMethod]
    [DataRow(true, 40)]
    [DataRow(false, 26400)]
    public void Day08_1(bool isExample, int expected)
    {
        var puzzle = new Day08();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle), isExample ? 10 : 1000));
    }

    [TestMethod]
    [DataRow(true, 25272)]
    [DataRow(false, 8199963486)]
    public void Day08_2(bool isExample, long expected)
    {
        var puzzle = new Day08();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle), isExample ? 20 : 1000));
    }

}