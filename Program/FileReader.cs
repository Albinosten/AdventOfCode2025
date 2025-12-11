
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
        public static void SaveResult(bool IsExample,Part part, object puzzle, int id,string result)
        {
			var fileName = IsExample
			? part.ToString()+id + "_Example.txt"
			: part.ToString()+id + ".txt";
			var path = $"{GetSourceDir()}/../Files/"+ puzzle.GetType().Name+"/";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (!File.Exists(path + "/" + fileName))
			{
				var s = File.Create(path + "/" + fileName);
                s.Close();
			}
            File.WriteAllText(path + "/" + fileName, result);
		}
		public static string? GetResult(bool IsExample,Part part, object puzzle, int id)
		{
			var fileName = IsExample
			? part.ToString()+id + "_Example.txt"
			: part.ToString()+id + ".txt";
			var path = $"{GetSourceDir()}/../Files/" + puzzle.GetType().Name ;

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
			if (!File.Exists(path + "/" + fileName))
			{
                return null;
			}
            return File.ReadAllLines(path + "/" + fileName).FirstOrDefault();
		}
		public static string GetSourceDir([System.Runtime.CompilerServices.CallerFilePath] string path = "")
        {
            return Path.GetDirectoryName(path) ?? "";
        }
    }
}