using Xunit;
using PhoneBook.ContactService.Features.ContactInfoFeature.Queries;
using PhoneBook.ContactService.Features.ContactInfoFeature.Commands;
using ContactService.DAL;
using EntityFrameworkCore.Testing.NSubstitute;
using System.Threading;
using System.Collections.Generic;
using ContactService.Models;
using System;
using PhoneBook.ContactService.Exceptions.TypedExceptions;
using PhoneBook.Library.Dto.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.ContactService.Tests
{
    public class ContactInfoFeatureTests
    {
        [Fact]
        public async void ContactInfoReportShouldReturnEmptyListWhenThereIsNoRecordInDatabase()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var personListQueryHandler = new ContactInfoReportQueryHandler(mockedDbContext);
            var result = await personListQueryHandler.Handle(new ContactInfoReportQuery(), CancellationToken.None);
            Assert.Empty(result);
        }

        [Fact]
        public async void ContactInfoReportShouldReturnNotEmptyListWhenThereIsSomeRecordsInDatabase()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Surname = "Test",
                Company = "Test"
            };

            var contactInfo = new ContactInfo()
            {
                Id = Guid.NewGuid(),
                PersonId = person.Id,
                Phone = "5555555555",
                Email = "test@gmail.com",
                Location = "Test"
            };
            await mockedDbContext.People.AddAsync(person);
            await mockedDbContext.ContactInfos.AddAsync(contactInfo);
            await mockedDbContext.SaveChangesAsync();

            var personListQueryHandler = new ContactInfoReportQueryHandler(mockedDbContext);
            var result = await personListQueryHandler.Handle(new ContactInfoReportQuery(), CancellationToken.None);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void ContactInfoPostShouldReturnAnErrorWhenCalledWithNotExistentPersonId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new ContactInfoPostCommandHandler(mockedDbContext);
            await Assert.ThrowsAsync<PersonNotFoundException>(async () => await handler.Handle(new ContactInfoPostCommand(), CancellationToken.None));
        }

        [Fact]
        public async void ContactInfoPostShouldReturnGuidWhenPersonIdExists()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Surname = "Test",
                Company = "Test"
            };

            await mockedDbContext.People.AddAsync(person);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ContactInfoPostCommandHandler(mockedDbContext);
            var command = new ContactInfoPostCommand() 
            { 
                PersonId = person.Id,
                Phone = "5555555555",
                Email = "test@gmail.com",
                Location = "Test"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            var contactInfo = await mockedDbContext.ContactInfos.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(contactInfo);
            Assert.Equal(command.PersonId, contactInfo.PersonId);
            Assert.Equal(command.Phone, contactInfo.Phone);
            Assert.Equal(command.Email, contactInfo.Email);
            Assert.Equal(command.Location, contactInfo.Location);
        }

        [Fact]
        public async void ContactInfoPutShouldReturnAnErrorWhenCalledWithNotExistentPersonId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new ContactInfoPutCommandHandler(mockedDbContext);
            await Assert.ThrowsAsync<PersonNotFoundException>(async () => await handler.Handle(new ContactInfoPutCommand(), CancellationToken.None));
        }
        
        [Fact]
        public async void ContactInfoPutShouldReturnAnErrorWhenCalledWithNotExistentContactInfoId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Surname = "Test",
                Company = "Test"
            };

            await mockedDbContext.People.AddAsync(person);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ContactInfoPutCommandHandler(mockedDbContext);
            await Assert.ThrowsAsync<ContactInfoNotFoundException>(async () => await handler.Handle(new ContactInfoPutCommand() { Id = Guid.NewGuid(), PersonId = person.Id }, CancellationToken.None));
        }
       
        [Fact]
        public async void ContactInfoPutShouldReturnGuidWhenAllValuesAreCorrect()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Surname = "Test",
                Company = "Test"
            };

            var contactInfo = new ContactInfo()
            {
                Id = Guid.NewGuid(),
                PersonId = person.Id,
                Phone = "5555555555",
                Email = "test@test.com",
                Location = "Test"
            };

            await mockedDbContext.People.AddAsync(person);
            await mockedDbContext.ContactInfos.AddAsync(contactInfo);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ContactInfoPutCommandHandler(mockedDbContext);
            var command = new ContactInfoPutCommand()
            {
                Id = contactInfo.Id,
                PersonId = person.Id,
                Phone = "5555555556",
                Email = "test2@gmail.com",
                Location = "Test2"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            var contactInfoNew = await mockedDbContext.ContactInfos.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(contactInfo);
            Assert.Equal(command.PersonId, contactInfoNew.PersonId);
            Assert.Equal(command.Phone, contactInfoNew.Phone);
            Assert.Equal(command.Email, contactInfoNew.Email);
            Assert.Equal(command.Location, contactInfoNew.Location);
        }

        [Fact]
        public async void ContactInfoDeleteShouldReturnAnErrorWhenCalledWithNotExistentId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new ContactInfoDeleteCommandHandler(mockedDbContext);
            await Assert.ThrowsAsync<ContactInfoNotFoundException>(async () => await handler.Handle(new ContactInfoDeleteCommand(), CancellationToken.None));
        }

        [Fact]
        public async void ContactInfoDeleteShouldReturnUnitWhenIdExists()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Surname = "Test",
                Company = "Test"
            };

            var contactInfo = new ContactInfo()
            {
                Id = Guid.NewGuid(),
                PersonId = person.Id,
                Phone = "5555555555",
                Email = "test@test.com",
                Location = "Test"
            };

            await mockedDbContext.People.AddAsync(person);
            await mockedDbContext.ContactInfos.AddAsync(contactInfo);
            await mockedDbContext.SaveChangesAsync();

            var handler = new ContactInfoDeleteCommandHandler(mockedDbContext);
            var result = await handler.Handle(new ContactInfoDeleteCommand() { Id = contactInfo.Id }, CancellationToken.None);
            Assert.IsType<Unit>(result);
        }
    }
}
