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

        public virtual Task<IEnumerable<Person>> SearchByNames(string searchText)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Person> Save(Person personToSave)
        {
            throw new NotImplementedException();
        }
    }
}
