using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleSearch
{
    public class PersonRepository
    {
        private readonly DbContextOptions<PersonContext> contextBuildOptions;
        private readonly DataGenerator dataGenerator;
        private readonly PeopleSearchSettings settings;

        public PersonRepository(
            DbContextOptions<PersonContext> contextBuildOptions,
            DataGenerator dataGenerator,
            PeopleSearchSettings settings)
        {
            this.contextBuildOptions = contextBuildOptions;
            this.dataGenerator = dataGenerator;
            this.settings = settings;
        }

        private PersonContext GetContext()
        {
            return new PersonContext(contextBuildOptions, dataGenerator, settings);
        }

        public virtual async Task<Person> Save(Person personToSave)
        {
            using (var context = GetContext())
            {
                await context.AddAsync(personToSave);
                await context.SaveChangesAsync();
                return personToSave;
            }
        }

        public virtual async Task<IList<Person>> SearchByNames(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new List<Person>();
            }

            var searchTerms = searchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Normalize(s))
                .ToList();

            if(searchTerms.Count < 2 || searchTerms.Count > 3)
            {
                throw new Exception("This search only supports 2-3 search terms");
            }

            var first = Normalize(searchTerms.First());
            var last = Normalize(searchTerms.Last());
            string middle;
            if (searchTerms.Count == 3)
            {
                middle = Normalize(searchTerms[1]);
            }
            else
            {
                middle = last; // we don't know if the search term is just an unfinished full name or first and last
            }

            var firstPoints = 24;
            var middlePoints = 20;
            var lastPoints = 22;

            var matchPoints = firstPoints / 2;
            var penalty = (-1 * matchPoints) + 1;

            var weight = 1;
            if (last == middle)
            {
                // don't award points twice for the same match
                weight = 2;
            }

            (Person person, int score) GetSearchScore(Person unNormalizedPerson)
            {
                var person = Normalize(unNormalizedPerson);
                int score = 0;
                var firstIsMatch = person.FirstName.Contains(first);
                var lastIsMatch = person.LastName.Contains(last);
                var middleIsMatch = person.MiddleName.Contains(middle);
                var matchCount =
                    (firstIsMatch ? 1 : 0) +
                    (lastIsMatch ? 1 : 0) +
                    (middleIsMatch ? 1 : 0);
                if (matchCount < 2)
                {
                    return (unNormalizedPerson, score);
                }

                score += firstIsMatch ? matchPoints : penalty;
                score += person.FirstName == first ? firstPoints : 0;

                score += lastIsMatch ? matchPoints : penalty / weight;
                score += person.LastName == last ? lastPoints / weight : 0;

                score += middleIsMatch ? matchPoints : penalty / weight;
                score += person.MiddleName == middle ? middlePoints / weight : 0;

                return (unNormalizedPerson, score);
            }

            using (var context = GetContext())
            {
                var people = (await context.People
                    .Where(x =>
                        EF.Functions.Like(x.FirstName, $"%{first}%") ||
                        EF.Functions.Like(x.LastName, $"%{last}%") ||
                        EF.Functions.Like(x.MiddleName, $"%{middle}%"))
                    .Include(x => x.Address)
                    .ToListAsync())
                    .Select(p => GetSearchScore(p))
                    .Where(p => p.score > 0)
                    .OrderByDescending(p => p.score)
                    .Select(p => p.person)
                    .ToList();
                return people;
            }
        }

        private string Normalize(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.Trim().ToLowerInvariant();
        }

        private Person Normalize(Person person)
        {
            return new Person
            {
                FirstName = Normalize(person.FirstName),
                MiddleName = Normalize(person.MiddleName),
                LastName = Normalize(person.LastName),
            };
        }
    }
}
