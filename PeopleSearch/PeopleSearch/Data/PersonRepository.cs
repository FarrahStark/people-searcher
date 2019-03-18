using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleSearch
{
    public class PersonRepository
    {
        public Task<IEnumerable<Person>> SearchByNames(string searchText)
        {
            throw new NotImplementedException();
        }

        public Task<Person> Save(Person personToSave)
        {
            throw new NotImplementedException();
        }
    }
}
