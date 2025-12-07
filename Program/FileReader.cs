
namespace AdventOfCode2025
{
    public class FileReader
    {
        public static IList<string> GetInput(bool IsExample, object puzzle)
        {
            var fileName = IsExample 
            ? puzzle.GetType().Name + "_Example.txt"
            : puzzle.GetType().Name + ".txt";
            var path = $"{GetSourceDir()}/../Files/"+fileName;

            if(!File.Exists(path))
            {
                var s = File.Create(path);
                s.Close();
            }
            var allLines = File.ReadAllLines(path);
            return allLines;
        }
        public static string GetSourceDir([System.Runtime.CompilerServices.CallerFilePath] string path = "")
        {
            return Path.GetDirectoryName(path) ?? "";
        }
    }
}