using AdventOfCode2025;

[TestClass]
public class Day03Tests
{   

    [TestMethod]
    [DataRow("987654321111111", 98)]
    [DataRow("811111111111119", 89)]

    public void Day03_1_Test(string bank, long expected)
    {
        var puzzle = new Day03();
        Assert.AreEqual(expected, puzzle.First([bank]));
    }
    [TestMethod]
    [DataRow("987654321111111", 987654321111)]
    [DataRow("811111111111119", 811111111119)]
    [DataRow("234234234234278", 434234234278)]
    [DataRow("818181911112111", 888911112111)]

    public void Day03_2_Test(string bank, long expected)
    {
        var puzzle = new Day03();
        Assert.AreEqual(expected, puzzle.Second([bank]));
    }
    [TestMethod]
    [DataRow(true, 357)]
    [DataRow(false, 17074)]
    public void Day03_1(bool isExample, long expected)
    {
        var puzzle = new Day03();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }
    [TestMethod]
    [DataRow(true, 3121910778619)]
    [DataRow(false, 169512729575727)]
    public void Day03_2(bool isExample, long expected)
    {
        var puzzle = new Day03();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)));
    }

}