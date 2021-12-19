namespace PhoneBook.ReportService.Services
{
    public interface IFileService
    {
        bool Exists(string? path);
    }

    public class FileService : IFileService
    {
        public bool Exists(string? path)
        {
            return File.Exists(path);
        }
    }
}
