namespace PhoneBook.ReportService.Models
{
    public class ReportInfo
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string? FilePath { get; set; }
    }
}
