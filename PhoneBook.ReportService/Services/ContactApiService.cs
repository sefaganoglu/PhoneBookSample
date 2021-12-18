using PhoneBook.Library.Dto.Response;
using PhoneBook.ReportService.Services.Configurations;

namespace PhoneBook.ReportService.Services
{
    public class ContactApiService
    {
        private readonly HttpClient _client;

        public ContactApiService(ContactApiConfig config)
        {
            var handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri(config.BaseUrl);
        }

        public async Task<List<ResContactInfoReportDto>> GetReport()
        {
            var response = await _client.GetAsync("ContactInfo/Report");
            var content = await response.Content.ReadAsAsync<List<ResContactInfoReportDto>>();
            return content;
        }
    }
}
