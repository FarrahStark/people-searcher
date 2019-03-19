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
            var nameFragments = searchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            using (var context = GetContext())
            {
                var people = await context.People
                    .Where(x => nameFragments.Any(n =>
                        x.FirstName.Contains(n) ||
                        x.LastName.Contains(n) ||
                        x.MiddleName.Contains(n)))
                    .Include(x => x.Address)
                    .ToListAsync();
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
    }
}
