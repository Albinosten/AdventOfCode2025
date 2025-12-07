using AdventOfCode2025;

[TestClass]
public class Day06Tests
{
    [TestMethod]
    [DataRow(true, 4277556)]
    [DataRow(false, 4693419406682)]
    public void Day06_1(bool isExample, long expected)
    {
        var puzzle = new Day06();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }
    
    [TestMethod]
    [DataRow(true, 3263827)]
    [DataRow(false, 9029931401920)]
    public void Day06_2(bool isExample, long expected)
    {
        var puzzle = new Day06();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }
}