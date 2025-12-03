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
    [DataRow("123123-123123", 123123)]
    [DataRow("11-22", 33)]
    [DataRow("998-1012", 2009)]
    [DataRow("1188511880-1188511890", 1188511885)]
    [DataRow("824-1475", 7947)]
    [DataRow("1010-1010", 1010)]
    public void Day02_2_Test(string input, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.Second(input));
    }

    [TestMethod]
    [DataRow(true, 4174379265)]
    [DataRow(false, 69564213293)]
    public void Day02_2(bool isExample, long expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.Second(FileReader.GetInput(isExample, puzzle)[0]));
    }

    [TestMethod]
    [DataRow("121212", 2, true)]
    [DataRow("121212", 2, true)]
    [DataRow("123123", 3, true)]
    [DataRow("123123", 2, false)]
    [DataRow("1011", 2, false)]
    [DataRow("1188511880", 4,false)]
    [DataRow("1001001", 3,false)]
    //1188511880
    //1001001,3,false
    
    public void StringCompare(string number, int patternSize, bool result)
    {
        var puzzle = new Day02();
        Assert.AreEqual(result, puzzle.StringCompare(number, patternSize));
    }
    
}