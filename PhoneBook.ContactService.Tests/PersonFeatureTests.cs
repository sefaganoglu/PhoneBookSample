using Xunit;
using PhoneBook.ContactService.Features.ProductFeature.Queries;
using ContactService.DAL;
using EntityFrameworkCore.Testing.NSubstitute;
using System.Threading;
using System.Collections.Generic;
using ContactService.Models;
using System;
using PhoneBook.ContactService.Exceptions.TypedExceptions;
using PhoneBook.Library.Dto.Response;
using PhoneBook.ContactService.Features.PersonFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.ContactService.Tests
{
    public class PersonFeatureTests
    {
        [Fact]
        public async void PersonListShouldReturnEmptyListWhenThereIsNoRecordInDatabase()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var personListQueryHandler = new PersonListQueryHandler(mockedDbContext);
            var result = await personListQueryHandler.Handle(new PersonListQuery(), CancellationToken.None);
            Assert.Empty(result);
        }

        [Fact]
        public async void PersonListShouldReturnNotEmptyListWhenThereIsSomeRecordsInDatabase()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            await mockedDbContext.People.AddRangeAsync(new List<Person>() {
                new Person
                {
                     Id = Guid.NewGuid(),
                     Name = "Test1",
                     Surname = "Test1",
                     Company = "Test1"
                },
                new Person
                {
                     Id = Guid.NewGuid(),
                     Name = "Test2",
                     Surname = "Test2",
                     Company = "Test2"
                }
            });
            await mockedDbContext.SaveChangesAsync();

            var handler = new PersonListQueryHandler(mockedDbContext);
            var result = await handler.Handle(new PersonListQuery(), CancellationToken.None);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void PersonGetShouldReturnAnErrorWhenCalledWithNotExistentId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new PersonGetQueryHandler(mockedDbContext);
            await Assert.ThrowsAsync<PersonNotFoundException>(async () => await handler.Handle(new PersonGetQuery(), CancellationToken.None));
        }

        [Fact]
        public async void PersonGetShouldReturnPersonDetailsWhenIdExists()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test1",
                Surname = "Test1",
                Company = "Test1"
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

            var handler = new PersonGetQueryHandler(mockedDbContext);
            var result = await handler.Handle(new PersonGetQuery() { Id = person.Id }, CancellationToken.None);
            Assert.IsType<ResPersonGetDto>(result);
            Assert.Equal(person.Id, result.Id);
        }

        [Fact]
        public async void PersonPostShouldReturnGuidWhenCompanyIsEmpty()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new PersonPostCommandHandler(mockedDbContext);

            var command = new PersonPostCommand()
            {
                Name = "Test",
                Surname = "Test",
                Company = ""
            };
            var result = await handler.Handle(command, CancellationToken.None);

            var person = await mockedDbContext.People.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(person);
            Assert.Equal(command.Name, person.Name);
            Assert.Equal(command.Surname, person.Surname);
            Assert.Equal(command.Company, person.Company);
        }

        [Fact]
        public async void PersonPostShouldReturnGuidWhenAllValuesAreCorrect()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new PersonPostCommandHandler(mockedDbContext);

            var command = new PersonPostCommand() { 
                Name = "Test", 
                Surname = "Test", 
                Company = "Test" 
            };

            var result = await handler.Handle(command, CancellationToken.None);

            var person = await mockedDbContext.People.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(person);
            Assert.Equal(command.Name, person.Name);
            Assert.Equal(command.Surname, person.Surname);
            Assert.Equal(command.Company, person.Company);
        }
        
        [Fact]
        public async void PersonPutShouldReturnAnErrorWhenCalledWithNotExistentId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new PersonPutCommandHandler(mockedDbContext);
            await Assert.ThrowsAsync<PersonNotFoundException>(async () => await handler.Handle(new PersonPutCommand(), CancellationToken.None));
        }

        [Fact]
        public async void PersonPutShouldReturnGuidWhenCompanyIsEmpty()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test1",
                Surname = "Test1",
                Company = "Test1"
            };
            await mockedDbContext.AddAsync(person);
            await mockedDbContext.SaveChangesAsync();

            var handler = new PersonPutCommandHandler(mockedDbContext);
            var personPutCommand = new PersonPutCommand() { 
                Id = person.Id,
                Name = "Test2",
                Surname = "Test2",
                Company = ""
            };

            var result = await handler.Handle(personPutCommand, CancellationToken.None);

            var personNew = await mockedDbContext.People.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(person);
            Assert.Equal(personPutCommand.Id, personNew.Id);
            Assert.Equal(personPutCommand.Name, personNew.Name);
            Assert.Equal(personPutCommand.Surname, personNew.Surname);
            Assert.Equal(personPutCommand.Company, personNew.Company);
        }

        [Fact]
        public async void PersonPutShouldReturnGuidWhenAllValuesAreCorrect()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Test1",
                Surname = "Test1",
                Company = "Test1"
            };
            await mockedDbContext.AddAsync(person);
            await mockedDbContext.SaveChangesAsync();

            var handler = new PersonPutCommandHandler(mockedDbContext);
            var personPutCommand = new PersonPutCommand()
            {
                Id = person.Id,
                Name = "Test2",
                Surname = "Test2",
                Company = "Test2"
            };

            var result = await handler.Handle(personPutCommand, CancellationToken.None);

            var personNew = await mockedDbContext.People.FirstOrDefaultAsync();

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(person);
            Assert.Equal(personPutCommand.Id, personNew.Id);
            Assert.Equal(personPutCommand.Name, personNew.Name);
            Assert.Equal(personPutCommand.Surname, personNew.Surname);
            Assert.Equal(personPutCommand.Company, personNew.Company);
        }

        [Fact]
        public async void PersonDeleteShouldReturnAnErrorWhenCalledWithNotExistentId()
        {
            var mockedDbContext = Create.MockedDbContextFor<ContactDbContext>();

            var handler = new PersonDeleteCommandHandler(mockedDbContext);
            await Assert.ThrowsAsync<PersonNotFoundException>(async () => await handler.Handle(new PersonDeleteCommand(), CancellationToken.None));
        }

        [Fact]
        public async void PersonDeleteShouldReturnUnitWhenIdExists()
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

            var handler = new PersonDeleteCommandHandler(mockedDbContext);
            var result = await handler.Handle(new PersonDeleteCommand() { Id = person.Id }, CancellationToken.None);
            Assert.IsType<Unit>(result);
        }
    }
}