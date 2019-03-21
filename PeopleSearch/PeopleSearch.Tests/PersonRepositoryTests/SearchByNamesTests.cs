using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PeopleSearch.Tests.PersonRepositoryTests
{
    public class SearchByNamesTests : InMemoryDatabaseTest
    {
        [Fact]
        public async Task Records_without_2_matches_are_ignored()
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: "Records without at 2 matches are ignored",
                searchText: "First Middle Last",
                people: new PersonInfo[]
                {
                    new PersonInfo("First", "Middleton", "Last", 1),
                    new PersonInfo("Firstish", "Middleton", "jasfdlkjaf", 2),
                    new PersonInfo("First", "asdf", "asdfafs", null),
                    new PersonInfo("asfasf", "Middle", "asdfafs", null),
                    new PersonInfo("asdf", "asdf", "Last", null),
                }
            );

            await ExecuteSearchTest(testCase);
        }

        [Fact]
        public async Task Correct_first_name_beats_correct_middle_name()
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: "Correct first name beats correct middle name",
                searchText: "First Middle Last",
                people: new PersonInfo[]
                {
                    new PersonInfo("First", "Jade", "Last", 1),
                    new PersonInfo("Henry", "Middle", "Last", 2)
                }
            );

            await ExecuteSearchTest(testCase);
        }

        [Fact]
        public async Task Correct_first_name_beats_correct_last_name()
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: "Correct first name beats correct last name",
                searchText: "First Middle Last",
                people: new PersonInfo[]
                {
                    new PersonInfo("First", "Middle", "Janeway", 1),
                    new PersonInfo("Henry", "Middle", "Last", 2)
                }
            );

            await ExecuteSearchTest(testCase);
        }

        [Fact]
        public async Task Correct_last_name_beats_correct_middle_name()
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: "Correct last name beats correct middle name",
                searchText: "First Middle Last",
                people: new PersonInfo[]
                {
                    new PersonInfo("First", "Jade", "Last", 1),
                    new PersonInfo("First", "Middle", "Jefferson", 2)
                }
            );

            await ExecuteSearchTest(testCase);
        }

        [Fact]
        public async Task Full_match_beats_partial_match()
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: "Full match beats partial match",
                searchText: "First Middle Last",
                people: new PersonInfo[]
                {
                    new PersonInfo("First", "Middle", "Last", 1),
                    new PersonInfo("First", "Middleton", "Last", 2)
                }
            );

            await ExecuteSearchTest(testCase);
        }

        [Theory]
        [InlineData(null, "null")]
        [InlineData("", "empty string for")]
        [InlineData("\t    \t", "whitespace for")]
        public async Task Blank_search_text_returns_an_empty_array(string searchText, string description)
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: $"{description} search text returns an empty array",
                searchText: searchText,
                people: new PersonInfo[] { }
            );

            await ExecuteSearchTest(testCase);
        }

        private async Task ExecuteSearchTest(TestCaseInfo testCase)
        {
            var people = await PersistPeople(testCase.People);
            var expectedPeople = people
                .Where(p => p.expectedRanking.HasValue)
                .OrderBy(p => p.expectedRanking)
                .ToArray();

            var results = (await PersonRepository.SearchByNames(testCase.SearchText)).ToArray();

            results.Length.ShouldBe(expectedPeople.Length,
                testCase.Details);
            for (int i = 0; i < expectedPeople.Length; ++i)
            {
                var actual = results[i];
                var expected = expectedPeople[i].person;
                actual.FirstName.ShouldBe(expected.FirstName, testCase.Details);
                actual.MiddleName.ShouldBe(expected.MiddleName, testCase.Details);
                actual.LastName.ShouldBe(expected.LastName, testCase.Details);
            }
        }

        private async Task<(Person person, int? expectedRanking)[]> PersistPeople(PersonInfo[] peopleInfo)
        {
            var people = peopleInfo.Select(p =>
            {
                var person = DataGenerator.GetPerson();
                person.FirstName = p.FirstName;
                person.MiddleName = p.MiddleName;
                person.LastName = p.LastName;
                return (person: person, expectedRanking: p.ExpectedRanking);
            }).Shuffle();

            foreach (var personDetails in people)
            {
                await PersonRepository.Save(personDetails.person);
            }

            return people;
        }

        public struct TestCaseInfo
        {
            public readonly string Details;
            public readonly string SearchText;
            public readonly PersonInfo[] People;
            public TestCaseInfo(string testCaseDetails, string searchText, PersonInfo[] people)
            {
                Details = testCaseDetails;
                SearchText = searchText;
                People = people;
            }
        }

        public struct PersonInfo
        {
            public readonly string FirstName;
            public readonly string MiddleName;
            public readonly string LastName;
            public readonly int? ExpectedRanking;

            public PersonInfo(string firstName, string middleName, string lastName, int? expectedRanking)
            {
                FirstName = firstName;
                MiddleName = middleName;
                LastName = lastName;
                ExpectedRanking = expectedRanking;
            }
        }
    }
}
