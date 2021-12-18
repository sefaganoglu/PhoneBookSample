using Newtonsoft.Json;
using PhoneBook.Library.Dto.Response;
using PhoneBook.ReportService.Services.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PhoneBook.ReportService.Services
{
    public class RabbitMQService
    {
        private readonly string queueCreateReport = "create_report_queue";
        private readonly string exchangeReportCreate = "report_create_exchange";
        IConnection Connection;
        public IModel Channel;

        private RabbitMQConfig _config;

        public RabbitMQService(RabbitMQConfig config)
        {
            _config = config;
        }

        private IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(_config.RabbitMQConnection)
            };

            return connectionFactory.CreateConnection();
        }     

        private IModel GetChannel()
        {
            return Connection.CreateModel();
        }

        public bool Connect()
        {
            if (Connection == null || !Connection.IsOpen)
            {
                Connection = GetConnection();
            }

            if (Channel == null || !Channel.IsOpen)
            {
                Channel = GetChannel();
            }

            Channel.ExchangeDeclare(exchangeReportCreate, "direct");
            Channel.QueueDeclare(queueCreateReport, false, false, false);
            Channel.QueueBind(queueCreateReport, exchangeReportCreate, queueCreateReport);

            return true;
        }

        public bool WriteToCreateReportQueue(Guid reportInfoId)
        {
            var messageArr = Encoding.UTF8.GetBytes(reportInfoId.ToString());

            Channel.BasicPublish(exchangeReportCreate, queueCreateReport, null, messageArr);

            return true;
        }

        public bool Consume(EventingBasicConsumer consumerEvent)
        {
            Channel.BasicConsume(queueCreateReport, false, consumerEvent);
            
            return true;
        }
    }
}
