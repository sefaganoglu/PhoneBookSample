using PhoneBook.ReportService.Services;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using PhoneBook.ReportService.DAL;
using PhoneBook.ReportService.Models;
using PhoneBook.Library.Dto.Response.Extensions;
using PhoneBook.Library.Helper;

namespace PhoneBook.ReportService.BackgroundServices
{
    [ApiExceptionAttribute]
    public class ReportBuilder : BackgroundService
    {
        private RabbitMQService _rabbitMQService;
        private ContactApiService _contactApiService;
        private IServiceScopeFactory _serviceScopeFactory;
        public ReportBuilder(IRabbitMQService rabbitMQManager, ContactApiService contactApiService, IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMQService = ((RabbitMQService)rabbitMQManager);
            _contactApiService = contactApiService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            StartConsumer();

            return Task.CompletedTask;
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                var reportInfoId = Guid.Parse(System.Text.Encoding.UTF8.GetString(e.Body.ToArray()));

                var filePath = "";
                var report = await _contactApiService.GetReport();
                string csvText = report.ToCsvFormat();

                filePath = Helper.SaveToCsvFile(csvText, "CsvFiles", reportInfoId.ToString());


                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ReportDbContext>();

                    var reportInfo = context.ReportInfos.Where(p => p.Id == reportInfoId).FirstOrDefault();
                    if (reportInfo != null)
                    {
                        reportInfo.Status = "Ready";
                        reportInfo.FilePath = filePath;

                        context.Entry(reportInfo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                }

                _rabbitMQService.Channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception)
            {
                _rabbitMQService.Channel.BasicNack(e.DeliveryTag, true, true);
                StartConsumer();
            }
        }

        EventingBasicConsumer consumer;
        private void StartConsumer()
        {
            _rabbitMQService.Connect();
            consumer = new EventingBasicConsumer(_rabbitMQService.Channel);
            consumer.Received += Consumer_Received;
            _rabbitMQService.Consume(consumer);
        }
    }
}
