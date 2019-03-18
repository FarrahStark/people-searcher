using System;
using Xunit;
using Moq;
using Shouldly;

namespace PeopleSearch.Tests.PersonTests
{
    public class Age
    {
        private static DateTime UtcDayThisYear(int month, int day)
        {
            return new DateTime(DateTime.Now.Year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        private static readonly DateTime UtcToday = UtcDayThisYear(6, 1);

        private static Mock<Person> MockPerson()
        {
            var mock = new Mock<Person>();
            mock.Setup(x => x.UtcToday)
                .Returns(UtcToday);
            return mock;
        }

        [Fact]
        public void When_BirthDay_is_before_today_age_is_correct()
        {
            //Arrange
            var person = MockPerson().Object;
            person.DateOfBirthUtc = UtcToday.AddDays(-1).AddYears(-50);
            const int expected = 50;
            //Act
            var actual = person.Age;
            //Assert
            expected.ShouldBe(actual);
        }

        [Fact]
        public void When_BirthDay_is_today_age_is_correct()
        {
            //Arrange
            var person = MockPerson().Object;
            person.DateOfBirthUtc = UtcToday.AddYears(-50);
            const int expected = 50;
            //Act
            var actual = person.Age;
            //Assert
            expected.ShouldBe(actual);
        }

        [Fact]
        public void When_BirthDay_is_after_today_age_is_correct()
        {
            //Arrange
            var person = MockPerson().Object;
            person.DateOfBirthUtc = UtcToday.AddDays(1).AddYears(-50);
            const int expected = 49;
            //Act
            var actual = person.Age;
            //Assert
            expected.ShouldBe(actual);
        }
    }
}
