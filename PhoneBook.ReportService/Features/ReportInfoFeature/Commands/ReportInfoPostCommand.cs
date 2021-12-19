using PhoneBook.ReportService.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ReportService.Models.Extensions;
using PhoneBook.Library.Dto.Response;
using PhoneBook.ReportService.Services;
using PhoneBook.ReportService.Models;
using PhoneBook.ReportService.Exceptions.TypedExceptions;

namespace PhoneBook.ReportService.Features.ReportInfoFeature.Commands
{
    public class ReportInfoPostCommand : IRequest<Guid>
    {

    }

    public class ReportInfoPostCommandHandler : IRequestHandler<ReportInfoPostCommand, Guid>
    {
        private readonly ReportDbContext _context;
        private IRabbitMQService _rabbitMQService;

        public ReportInfoPostCommandHandler(ReportDbContext context, IRabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<Guid> Handle(ReportInfoPostCommand request, CancellationToken cancellationToken)
        {
            await _context.Database.BeginTransactionAsync();
            try
            {
                var reportInfo = new ReportInfo()
                {
                    Id = Guid.NewGuid(),
                    RequestDate = DateTime.UtcNow,
                    Status = "Waiting"
                };

                await _context.ReportInfos.AddAsync(reportInfo);
                _context.Entry(reportInfo).State = EntityState.Added;
                await _context.SaveChangesAsync();

                if (_rabbitMQService.Connect() && _rabbitMQService.WriteToCreateReportQueue(reportInfo.Id))
                {
                    await _context.Database.CommitTransactionAsync();

                    return reportInfo.Id;
                }
                else
                {
                    throw new RabbitMqUnavailableException();
                }
            }
            catch (Exception)
            {
                await _context.Database.RollbackTransactionAsync();

                throw new RabbitMqUnavailableException();
            }
        }
    }
}
