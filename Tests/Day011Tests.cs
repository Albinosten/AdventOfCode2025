using AdventOfCode2025;

[TestClass]
public class Day11Tests
{
    [TestMethod]
    [DataRow(true, 5)]
    [DataRow(false, 543)]
    public void Day11_1(bool isExample, int expected)
    {
        var puzzle = new Day11();
        Assert.AreEqual(expected, puzzle.First(FileReader.GetInput(isExample, puzzle)));
    }

    [TestMethod]
    [DataRow(true, 2)]
    [DataRow(false, 0)]
    public void Day11_2(bool isExample, int expected)
    {
        var puzzle = new Day11();
        var input = FileReader.GetInput(isExample, puzzle);

		if (isExample)
        {
            input = this.GetExampleInputPart2();
        }
        Assert.AreEqual(expected, puzzle.Second(input));
    }
    public List<string> GetExampleInputPart2()
    {
        return new List<string>()
        {


"svr: aaa bbb",
"aaa: fft",
"fft: ccc",
"bbb: tty",
"tty: ccc",
"ccc: ddd eee",
"ddd: hub",
"hub: fff",
"eee: dac",
"dac: fff",
"fff: ggg hhh",
"ggg: out",
"hhh: out",

        };
    }
}
/*
 svr: aaa bbb
aaa: fft
fft: ccc
bbb: tty
tty: ccc
ccc: ddd eee
ddd: hub
hub: fff
eee: dac
dac: fff
fff: ggg hhh
ggg: out
hhh: out
*/