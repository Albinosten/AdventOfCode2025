using AdventOfCode2025;

[TestClass]
public class Day01Tests
{
    [TestMethod]
    [DataRow(true, 3)]
    [DataRow(false, 1086)]
    public void Day01_1(bool isExample, int expected)
    {
        var puzzle = new Day01();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }

    [TestMethod]
    [DataRow(true, 6)]
    [DataRow(false, 6268)]
    public void Day01_2(bool isExample, int expected)
    {
        var puzzle = new Day01();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }
}