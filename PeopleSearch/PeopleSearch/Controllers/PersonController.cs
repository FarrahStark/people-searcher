using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeopleSearch
{
    public class PersonController : Controller
    {
        private readonly PersonRepository personRepository;

        public PersonController(PersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IList<Person>> Search(string searchText, int? delayMilliseconds = null)
        {
            if (delayMilliseconds.HasValue)
            {
                await Task.Delay(delayMilliseconds.Value);
            }

            return await personRepository.SearchByNames(searchText);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<Person> Save(Person person)
        {
            return personRepository.Save(person);
        }
    }
}