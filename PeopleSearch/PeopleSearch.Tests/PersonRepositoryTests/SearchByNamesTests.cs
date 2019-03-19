using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PeopleSearch.Tests.PersonRepositoryTests
{
    public class SearchByNamesTests : InMemoryDatabaseTest
    {
        [Fact]
        public async Task The_results_are_returned_in_the_correct_order()
        {
            var testCase = new TestCaseInfo
            (
                testCaseDetails: "Search results are in the correct order",
                searchText: "First Middle Last",
                people: new PersonInfo[]
                {
                    new PersonInfo("First", "Middle", "Last", 1), // best match wins
                    new PersonInfo("First", "Jade", "Last", 2), // correct last name beats correct middle name
                    new PersonInfo("First", "Middle", "Jefferson", 3), // 2 name matches beats 1 name match
                    new PersonInfo("First", "Jose", "Jefferson", 4), // correct first name beats correct last name
                    new PersonInfo("Holly", "Hilda", "Last", 5), // correct last name beats correct middle name
                    new PersonInfo("Sara", "Middle", "Graph", 6), // exact match beats partial match
                    new PersonInfo("Monster", "Middleton", "Frankenstein", 7), // partial matches are returned
                    new PersonInfo("Sally", "Josephine", "Jefferson", (int?)null), // no match doesn't return
                    new PersonInfo("Jake", "The", "Snake", (int?)null) // no match doesn't return
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
                    new PersonInfo("First", "Jade", "Janeway", 1),
                    new PersonInfo("Henry", "Middle", "Jefferson", 2)
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
                    new PersonInfo("First", "Jade", "Janeway", 1),
                    new PersonInfo("Henry", "Salse", "Last", 2)
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
                    new PersonInfo("Jim", "Jade", "Last", 1),
                    new PersonInfo("Henry", "Middle", "Jefferson", 2)
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
                    new PersonInfo("Jim", "Middle", "Starkiller", 1),
                    new PersonInfo("Henry", "Middleton", "Jefferson", 2)
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
