using EntityFrameworkCore.Testing.NSubstitute;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PhoneBook.Library.Exceptions;
using PhoneBook.ReportService.DAL;
using PhoneBook.ReportService.Exceptions.TypedExceptions;
using PhoneBook.ReportService.Features.ReportInfoFeature.Commands;
using PhoneBook.ReportService.Features.ReportInfoFeature.Queries;
using PhoneBook.ReportService.Models;
using PhoneBook.ReportService.Services;
using PhoneBook.ReportService.Services.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace PhoneBook.ReportService.Test
{
    public class ReportFeatureTest
    {
        [Fact]
        public async void ReportListShouldReturnEmptyListWhenThereIsNoRecordInDatabase()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();

            var personListQueryHandler = new ReportInfoListQueryHandler(mockedDbContext);
            var result = await personListQueryHandler.Handle(new ReportInfoListQuery(), CancellationToken.None);
            Assert.Empty(result);
        }

        [Fact]
        public async void ReportListShouldReturnNotEmptyListWhenThereIsSomeRecordsInDatabase()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();

            await mockedDbContext.ReportInfos.AddRangeAsync(new List<ReportInfo>() {
                new ReportInfo
                {
                     Id = Guid.NewGuid(),
                     RequestDate = DateTime.UtcNow,
                     Status = "Waiting"
                },
                new ReportInfo
                {
                     Id = Guid.NewGuid(),
                     RequestDate = DateTime.UtcNow,
                     Status = "Waiting"
                }
            });
            await mockedDbContext.SaveChangesAsync();

            var handler = new ReportInfoListQueryHandler(mockedDbContext);
            var result = await handler.Handle(new ReportInfoListQuery(), CancellationToken.None);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void ReportInfoPostShouldReturnGuidWhenRabbitMqIsAvailable()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();
            var mockedRabbitMq = Substitute.For<IRabbitMQService>();
            mockedRabbitMq.Connect().ReturnsForAnyArgs(true);
            mockedRabbitMq.WriteToCreateReportQueue(default).ReturnsForAnyArgs(true);

            var handler = new ReportInfoPostCommandHandler(mockedDbContext, mockedRabbitMq);

            var result = await handler.Handle(new ReportInfoPostCommand(), CancellationToken.None);

            var reportInfo = await mockedDbContext.ReportInfos.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(reportInfo);
        }

        [Fact]
        public async void ReportInfoPostShouldReturnAnErrorWhenRabbitMqIsNotAvailable()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();
            var mockedRabbitMq = Substitute.For<IRabbitMQService>();

            mockedRabbitMq.Connect().ReturnsForAnyArgs(false);

            var handler = new ReportInfoPostCommandHandler(mockedDbContext, mockedRabbitMq);

            await Assert.ThrowsAsync<RabbitMqUnavailableException>(async () => await handler.Handle(new ReportInfoPostCommand(), CancellationToken.None));
        }

        [Fact]
        public async void ReportInfoDownloadShouldReturnAnErrorWhenCalledWithNotExistentId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();
            var mockedFile = Substitute.For<IFileService>();

            var handler = new ReportInfoDownloadQueryHandler(mockedDbContext, mockedFile);
            await Assert.ThrowsAsync<ReportInfoNotFoundException>(async () => await handler.Handle(new ReportInfoDownloadQuery(), CancellationToken.None));
        }

        [Fact]
        public async void ReportInfoDownloadShouldReturnAnErrorWhenReportIsNotReady()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();
            var mockedFile = Substitute.For<IFileService>();

            var reportInfo = new ReportInfo
            {
                Id = Guid.NewGuid(),
                RequestDate = DateTime.UtcNow,
                Status = "Waiting"
            };
            await mockedDbContext.ReportInfos.AddAsync(reportInfo);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ReportInfoDownloadQueryHandler(mockedDbContext, mockedFile);
            await Assert.ThrowsAsync<ReportIsNotReadyYetException>(async () => await handler.Handle(new ReportInfoDownloadQuery() { Id = reportInfo.Id }, CancellationToken.None));
        }

        [Fact]
        public async void ReportInfoDownloadShouldReturnAnErrorWhenFileNotFound()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();
            var mockedFile = Substitute.For<IFileService>();
            mockedFile.Exists(default).ReturnsForAnyArgs(false);

            var reportInfo = new ReportInfo
            {
                Id = Guid.NewGuid(),
                RequestDate = DateTime.UtcNow,
                Status = "Ready",
                FilePath = "CsvFiles\\Test.csv"
            };
            await mockedDbContext.ReportInfos.AddAsync(reportInfo);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ReportInfoDownloadQueryHandler(mockedDbContext, mockedFile);
            await Assert.ThrowsAsync<ReportFileNotFoundException>(async () => await handler.Handle(new ReportInfoDownloadQuery() { Id = reportInfo.Id }, CancellationToken.None));
        }

        [Fact]
        public async void ReportInfoDownloadShouldReturnFileInfoWhenIdExist()
        {
            var mockedDbContext = Create.MockedDbContextFor<ReportDbContext>();
            
            var mockedFile = Substitute.For<IFileService>();

            mockedFile.Exists(default).ReturnsForAnyArgs(true);

            var reportInfo = new ReportInfo
            {
                Id = Guid.NewGuid(),
                RequestDate = DateTime.UtcNow,
                Status = "Ready",
                FilePath = "CsvFiles\\Test.csv"
            };
            await mockedDbContext.ReportInfos.AddAsync(reportInfo);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ReportInfoDownloadQueryHandler(mockedDbContext, mockedFile);

            var fileInfo = await handler.Handle(new ReportInfoDownloadQuery() { Id = reportInfo.Id }, CancellationToken.None);

            Assert.IsType<ReportInfoDownloadQueryResult>(fileInfo);
            Assert.Equal(reportInfo.FilePath, fileInfo.FilePath);
        }
    }
}