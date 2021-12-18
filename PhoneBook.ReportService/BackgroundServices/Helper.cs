using System.Text;

namespace PhoneBook.ReportService.BackgroundServices
{
    public static class Helper
    {
        public static void CreateDirectoryIfNotExist(string path)
        {
            string[] splitted = path.Split('\\');
            string nextPath = "";
            for (int i = 0; i < splitted.Length; i++)
            {
                nextPath += splitted[i] + "\\";
                if (!Directory.Exists(nextPath))
                {
                    Directory.CreateDirectory(nextPath);
                }
            }
        }

        public static string SaveToCsvFile(string csvText, string path, string fileName)
        {
            CreateDirectoryIfNotExist(path);
            string filePath = Path.Combine(path, $"{ fileName }.csv");

            using (var file = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                file.Write(csvText);
                file.Close();
            }
            return filePath;
        }
    }
}
