using Shouldly;
using System.Linq;
using Xunit;

namespace PeopleSearch.Tests.PersonTests
{
    public class InterestsJson
    {
        [Fact]
        public void When_Interests_is_null_an_empty_json_array_is_returned()
        {
            //Arrange
            var person = new Person();
            person.Interests = null;
            const string expected = "[]";
            //Act
            var actual = person.InterestsJson;
            //Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void When_Interests_has_items_a_json_array_is_returned()
        {
            //Arrange
            var person = new Person();
            person.Interests = new[]
                {
                        "Football",
                        "Painting",
                        "Cooking"
                    };
            const string expected = "[\"Football\",\"Painting\",\"Cooking\"]";
            //Act
            var actual = person.InterestsJson;
            //Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void When_setting_InterestsJson_with_null_Interests_is_an_empty_enumerable()
        {
            //Arrange
            var person = new Person();
            person.Interests = new[]
            {
                    "Football",
                    "Painting",
                    "Cooking"
                };
            //Act
            person.InterestsJson = null;
            var actual = person.Interests;
            //Assert
            (actual != null && !actual.Any()).ShouldBeTrue();
        }

        [Fact]
        public void When_setting_InterestsJson_with_emptyString_Interests_is_an_empty_enumerable()
        {
            //Arrange
            var person = new Person();
            person.Interests = new[]
            {
                    "Football",
                    "Painting",
                    "Cooking"
                };
            //Act
            person.InterestsJson = string.Empty;
            var actual = person.Interests;
            //Assert
            (actual != null && !actual.Any()).ShouldBeTrue();
        }

        [Fact]
        public void When_setting_InterestsJson_with_a_valid_json_array_Interests_is_an_equivalent_collection()
        {
            //Arrange
            var person = new Person();
            person.Interests = new string[] { };
            var expected = new[]
            {
                    "Football",
                    "Painting",
                    "Cooking"
                };
            //Act
            person.InterestsJson = "[\"Football\",\"Painting\",\"Cooking\"]";
            var actual = person.Interests;
            //Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Changing_the_order_of_items_triggers_a_change()
        {
            //Arrange
            var person = new Person();
            person.Interests = new string[] { };
            var expected = new[]
            {
                    "Football",
                    "Art",
                    "Cooking"
                };
            //Act
            person.InterestsJson = "[\"Cooking\",\"Football\",\"Art\"]";
            var actual = person.Interests;
            //Assert
            actual.ShouldNotBe(expected);
        }
    }
}
