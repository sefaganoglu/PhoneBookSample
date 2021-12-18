using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ReportService.Models.Extensions
{
    public static class ReportInfoExtensions
    {
        public static ResReportInfoListDto ToListDto(this ReportInfo reportInfo)
        {
            var dto = new ResReportInfoListDto()
            {
                Id = reportInfo.Id,
                RequestDate = reportInfo.RequestDate,
                Status = reportInfo.Status
            };

            return dto;
        }
    }
}
