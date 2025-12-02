using AdventOfCode2025;

[TestClass]
public class Day02Tests
{   
    [TestMethod]
    [DataRow(true, 1227775554)]
    [DataRow(false, 52316131093)]
    public void Day02_1(bool isExample, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }
    [TestMethod]
    [DataRow("11-22", 33)]
    [DataRow("998-1012", 2009)]
    [DataRow("1188511880-1188511890", 1188511885)]
    public void Day02_2_Examples(string input, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.Second([input]));
    }

    [TestMethod]
    [DataRow(true, 4174379265)]
    [DataRow(false, 69564213293)]
    public void Day02_2(bool isExample, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }
}