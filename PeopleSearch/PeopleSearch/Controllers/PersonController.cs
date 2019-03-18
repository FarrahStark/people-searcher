using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public Task<ActionResult> Search(string searchText, int? delayMilliseconds = null)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<ActionResult> Save(Person person)
        {
            throw new NotImplementedException();
        }
    }
}