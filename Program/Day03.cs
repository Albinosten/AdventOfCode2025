namespace AdventOfCode2025
{
    public class Day03
    {
        public long First(IList<string> input)
        {
            long result = 0;
            foreach(var number in input)
            {
                result += long.Parse(this.GetBiggest(number,2));
            }
            return result;
        }
        public long Second(IList<string> input)
        {
            long result = 0;
            foreach(var number in input)
            {
                result += long.Parse(this.GetBiggest(number, 12));
            }

            return result;
        }
        public char[] GetBiggest(string input, int count)
        {
            int indexOfBiggest = 0;
            for(int i = 0; i <= input.Length - count; i++)
            {
                if(input[i]>input[indexOfBiggest])
                {
                    indexOfBiggest = i;
                }
            }
            if(count == 1){return [input[indexOfBiggest]];}
            return [input[indexOfBiggest], ..GetBiggest(input[(indexOfBiggest + 1)..], count-1)];
        }
    }
}