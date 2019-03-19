using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<object> Search(string searchText, int takeCount = 100, int? delayMilliseconds = null)
        {
            if (delayMilliseconds.HasValue)
            {
                await Task.Delay(delayMilliseconds.Value);
            }

            var people = await personRepository.SearchByNames(searchText);
            return new { MatchingPeople = people.Take(takeCount) };
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<Person> Save(Person person)
        {
            return personRepository.Save(person);
        }
    }
}