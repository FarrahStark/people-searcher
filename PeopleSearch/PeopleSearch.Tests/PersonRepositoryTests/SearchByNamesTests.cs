using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PeopleSearch.Tests.PersonRepositoryTests
{
    public class SearchByNamesTests
    {
        private readonly PersonRepository personRepository;

        private SearchByNamesTests()
        {
            personRepository = new PersonRepository();
        }

        public static IEnumerable<object[]> SearchPrioritizationData = new List<object[]>
        {
            new object []
            {
                "Search results are in the correct order", // Test Case Details
                "First Middle Last", // Search Text
                new [] // Ranked Search Results
                {
                    ("First", "Middle", "Last", 1), // best match wins
                    ("First", "Jade", "Last", 2), // correct last name beats correct middle name
                    ("First", "Middle", "Jefferson", 3), // 2 name matches beats 1 name match
                    ("First", "Jose", "Jefferson", 4), // correct first name beats correct last name
                    ("Holly", "Hilda", "Last", 5), // correct last name beats correct middle name
                    ("Sara", "Middle", "Graph", 6), // exact match beats partial match
                    ("Monster", "Middleton", "Frankenstein", 7), // partial matches are returned
                    ("Sally", "Josephine", "Jefferson", (int?)null), // no match doesn't return
                    ("Jake", "The", "Snake", (int?)null) // no match doesn't return
                }
            },
            new object []
            {
                "Correct first name beats correct middle name", // Test Case Details
                "First Middle Last", // Search Text
                new [] // Ranked Search Results
                {
                    ("First", "Jade", "Janeway", 1),
                    ("Henry", "Middle", "Jefferson", 2)
                }
            },
            new object []
            {
                "Correct first name beats correct last name", // Test Case Details
                "First Middle Last", // Search Text
                new [] // Ranked Search Results
                {
                    ("First", "Jade", "Janeway", 1),
                    ("Henry", "Middle", "Last", 2)
                }
            },
            new object []
            {
                "Correct last name beats correct middle name", // Test Case Details
                "First Middle Last", // Search Text
                new [] // Ranked Search Results
                {
                    ("Jim", "Jade", "Last", 1),
                    ("Henry", "Middle", "Jefferson", 2)
                }
            },
            new object []
            {
                "Full match beats partial match", // Test Case Details
                "First Middle Last", // Search Text
                new [] // Ranked Search Results
                {
                    ("Jim", "Middle", "Starkiller", 1),
                    ("Henry", "Middleton", "Jefferson", 2)
                }
            },
            new object []
            {
                "null search text returns an empty array", // Test Case Details
                null, // Search Text
                new (string firstName, string middleName, string lastName, int? expectedRanking)[]
                {
                }
            },
            new object []
            {
                "empty string search text returns an empty array", // Test Case Details
                "", // Search Text
                new (string firstName, string middleName, string lastName, int? expectedRanking)[]
                {
                }
            },
            new object []
            {
                "whitespace string search text returns an empty array", // Test Case Details
                "\t ", // Search Text
                new (string firstName, string middleName, string lastName, int? expectedRanking)[]
                {
                }
            },
        };

        [Theory]
        [MemberData(nameof(SearchPrioritizationData))]
        public async Task When_searching_the_results_are_returned_in_the_correct_order(
            string testCaseDescription,
            string searchText,
            (string firstName, string middleName, string lastName, int? expectedRanking)[] peopleInfo)
        {
            var people = await PersistPeople(peopleInfo);
            var expectedPeople = people
                .Where(p => p.expectedRanking.HasValue)
                .OrderBy(p => p.expectedRanking)
                .ToArray();

            var results = (await personRepository.SearchByNames(searchText)).ToArray();

            results.Length.ShouldBe(expectedPeople.Length,
                testCaseDescription);
            for(int i = 0; i < expectedPeople.Length; ++i)
            {
                var actual = results[i];
                var expected = expectedPeople[i].person;
                actual.FirstName.ShouldBe(expected.FirstName, testCaseDescription);
                actual.MiddleName.ShouldBe(expected.MiddleName, testCaseDescription);
                actual.LastName.ShouldBe(expected.LastName, testCaseDescription);
            }
        }

        private Task<(Person person, int? expectedRanking)[]> PersistPeople(
            (string firstName, string middleName, string lastName, int? expectedRanking)[] peopleInfo)
        {
            var people = peopleInfo.Select(p =>
            {
                var person = DataGenerator.GetPerson();
                person.FirstName = p.firstName;
                person.MiddleName = p.middleName;
                person.LastName = p.lastName;
                return (person: person, expectedRanking: p.expectedRanking);
            }).Shuffle();

            // TODO: insert people here

            return Task.FromResult(people);
        }
    }
}
