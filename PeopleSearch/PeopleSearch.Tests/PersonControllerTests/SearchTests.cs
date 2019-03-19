using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PeopleSearch.Tests.PersonControllerTests
{
    public class SearchTests
    {
        private readonly PersonRepository personRepository;
        private readonly PersonController personController;
        private readonly Mock<PersonRepository> personRepositoryMock;

        public SearchTests()
        {
            personRepositoryMock = new Mock<PersonRepository>(
                new Mock<DbContextOptions<PersonContext>>().Object,
                new Mock<DataGenerator>().Object,
                new PeopleSearchSettings());
            personRepositoryMock
                .Setup(r => r.Save(It.IsAny<Person>()))
                .Returns(Task.FromResult(new Person()))
                .Verifiable();
            personRepositoryMock
                .Setup(r => r.SearchByNames(It.IsAny<string>()))
                .Returns(Task.FromResult((IList<Person>)new List<Person>()))
                .Verifiable();

            personRepository = personRepositoryMock.Object;
            personController = new PersonController(personRepository);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(100)]
        [InlineData(3000)]
        [InlineData(2)]
        public async Task The_search_takes_at_least_as_long_as_the_delay(int? delayMilliseconds)
        {
            var start = DateTime.UtcNow;
            var searchText = "John J. Jingleheimerschmidt";
            var searchResults = await personController.Search(searchText, delayMilliseconds: delayMilliseconds);
            var end = DateTime.UtcNow;
            var elapsed = (end - start).TotalMilliseconds;
            var expectedDelay = (delayMilliseconds.HasValue ? delayMilliseconds.Value : 0) * 1.0d;
            (elapsed >= expectedDelay).ShouldBeTrue();
        }

        [Fact]
        public async Task Search_calls_search_on_PersonRepository()
        {
            var searchText = "Indiana Jones";
            var results = await personController.Search(searchText);
            personRepositoryMock.Verify(r => r.SearchByNames(searchText), Times.Once);
        }

        [Fact]
        public async Task Save_calls_save_on_PersonRepository()
        {
            var person = new Mock<Person>().Object;
            var results = await personController.Save(person);
            personRepositoryMock.Verify(r => r.Save(person), Times.Once);
        }
    }
}
