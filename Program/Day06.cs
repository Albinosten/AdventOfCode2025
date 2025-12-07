namespace AdventOfCode2025
{
    public class Day06
    {
        public long First(IList<string> input)
        {
            var operators = new List<char>();
            var values = new List<long>();
            for(int i = input.Count-1; i >= 0; i--)
            {
                var split = input[i]
                    .Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.ToString())
                    .ToList();
                    ;
                for(int j = 0; j < split.Count;j++)
                {
                    if(i == input.Count-1)
                    {
                        operators.Add(split[j][0]);
                    }
                    else
                    {
                        if(values.Count <= j)
                        {
                            values.Add(0);
                        }
                        values[j] = Calculate(values[j], long.Parse( split[j]), operators[j]);
                    }
                }
            }
            return values.Sum();
        }
        public long Calculate(long number1, long number2, char op) => op switch 
        {
            '*' => number1 != 0 ? number1 * number2 : number2,
            '+' => number1 + number2,
            _ => throw new InvalidOperationException(),
        };

        public long Second(IList<string> input)
        {
            var numbers = new List<long>();
            var result = 0L;
            for(int i = input[0].Length-1; i >= 0; i--)
            {
                string number = "";
                for(int r = 0; r < input.Count-1; r++)
                {
                    number += input[r][i];
                }
                number = number.Replace(" ", "");
                
                if(!string.IsNullOrWhiteSpace(number))
                {
                    numbers.Add(long.Parse(number));
                }
                if(input[input.Count-1][i] == '+' || input[input.Count-1][i] == '*')
                {
                    var value = 0L;
                    foreach(var n in numbers)
                    {
                        value = Calculate(value, n, input[input.Count-1][i]);
                    }
                    numbers.Clear();
                    result += value;
                }
            }
            return result;
        }
    }
}