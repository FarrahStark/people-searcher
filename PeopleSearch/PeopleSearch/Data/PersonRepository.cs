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

        public PersonRepository(DbContextOptions<PersonContext> contextBuildOptions)
        {
            this.contextBuildOptions = contextBuildOptions;
        }

        private PersonContext GetContext()
        {
            return new PersonContext(contextBuildOptions);
        }

        public virtual async Task<IList<Person>> SearchByNames(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new List<Person>();
            }

            var searchTerms = searchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => normalize(s))
                .ToList();

            var first = searchTerms.FirstOrDefault();
            var last = searchTerms.LastOrDefault();
            var middleNames = new List<string>();
            for (int i = 1; i < searchTerms.Count - 1 && searchTerms.Count > 2; ++i)
            {
                middleNames.Add(searchTerms[i]);
            }

            using (var context = GetContext())
            {
                var people = (await context.People
                    .Where(x => searchTerms.Any(n =>
                        EF.Functions.Like(x.FirstName, $"%{n}%") ||
                        EF.Functions.Like(x.LastName, $"%{n}%") ||
                        EF.Functions.Like(x.MiddleName, $"%{n}%")))
                    .Include(x => x.Address)
                    .ToListAsync())
                    .Select(p => GetSearchScore(first, last, middleNames, searchTerms, p))
                    .OrderByDescending(p => p.score)
                    .Select(p => p.person)
                    .ToList();
                return people;
            }
        }

        public virtual async Task<Person> Save(Person personToSave)
        {
            using(var context = GetContext())
            {
                await context.AddAsync(personToSave);
                await context.SaveChangesAsync();
                return personToSave;
            }
        }

        private (Person person, int score) GetSearchScore(
                string searchFirstName,
                string searchLastName,
                List<string> searchMiddleNames,
                List<string> searchTermList,
                Person person)
        {
            var normalizedPersonFirst = normalize(person.FirstName);
            var normalizedPersonMiddle = normalize(person.MiddleName);
            var normalizedPersonLast = normalize(person.LastName);
            int score = 0;
            int increment = searchMiddleNames.Count;
            if (searchTermList.Any(s => normalizedPersonFirst.Contains(s)) ||
                searchTermList.Any(s => normalizedPersonMiddle.Contains(s)) ||
                searchTermList.Any(s => normalizedPersonLast.Contains(s)))
            {
                score += increment;
            }
            else
            {
                return (person, score);
            }

            if (searchTermList.Any(s => normalizedPersonFirst == s) ||
                searchTermList.Any(s => normalizedPersonMiddle == s) ||
                searchTermList.Any(s => normalizedPersonLast == s))
            {
                score += (increment + 1);
            }
            else
            {
                return (person, score);
            }

            if (normalizedPersonFirst.Contains(searchFirstName))
            {
                score += (increment + 2);
                if (normalizedPersonFirst == searchFirstName)
                {
                    score += 1;
                }
            }

            if (normalizedPersonLast.Contains(searchLastName))
            {
                score += (increment + 1);
                if (normalizedPersonLast == searchLastName)
                {
                    score += 1;
                }
            }

            if (normalizedPersonMiddle.Contains(searchLastName))
            {
                score += (increment + 1);
            }

            foreach (var middle in searchMiddleNames)
            {
                if (normalizedPersonMiddle.Contains(middle))
                {
                    score += 1;
                }
            }

            return (person, score);
        }

        private string normalize(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.Trim().ToLowerInvariant();
        }
    }
}
