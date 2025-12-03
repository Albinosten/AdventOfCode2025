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
    [DataRow(123123, 3, 1001)]
    [DataRow(12121212, 2,1010101)]
    public void GetBitmapTest(int number, int repeatingCount, int expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.GetBitMap(number.ToString().Length, repeatingCount));
    }
    [TestMethod]
    [DataRow(123123, 6)]
    [DataRow(12121212, 8)]
    [DataRow(3, 1)]
    public void DigitCount(int number, int expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.DigitCount(number));
    }
    [TestMethod]
    [DataRow(123456,3, 456)]
    [DataRow(12121212, 5, 21212)]
    
    public void GetPattern(int number, int count, int expected)
    {
        var puzzle = new Day02();
        Assert.AreEqual(expected, puzzle.GetPattern(number,count));
    }

    
}