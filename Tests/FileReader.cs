public class FileReader
{
    public static IList<string> GetInput(bool IsExample, object puzzle)
    {
        var fileName = IsExample 
        ? puzzle.GetType().Name + "_Example.txt"
        : puzzle.GetType().Name + ".txt";

        var allLines = File.ReadAllLines($"{GetSourceDir()}/../Files/"+fileName);
        return allLines;
    }
    public static string GetSourceDir([System.Runtime.CompilerServices.CallerFilePath] string path = "")
    {
        return Path.GetDirectoryName(path) ?? "";
    }
}