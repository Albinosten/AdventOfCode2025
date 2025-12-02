using AdventOfCode2025;

[TestClass]
public class Tests
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
    
    [TestMethod]
    [DataRow(true, 1227775554)]
    [DataRow(false, 52316131093)]
    public void Day02_1(bool isExample, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }

    [TestMethod]
    [DataRow(true, 4174379265)]
    [DataRow(false, 0)]
    public void Day02_2(bool isExample, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }
}