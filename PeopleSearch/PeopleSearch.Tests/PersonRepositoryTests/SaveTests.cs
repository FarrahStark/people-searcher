using Newtonsoft.Json;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PeopleSearch.Tests.PersonRepositoryTests
{
    public class SaveTests
    {
        private readonly PersonRepository personRepository;

        private SaveTests()
        {
            personRepository = new PersonRepository();
        }

        [Fact]
        public async Task When_save_is_called_the_person_is_persisted_with_an_id()
        {
            var person = DataGenerator.GetPerson();
            person.PersonId = 0;
            var savedPerson = await personRepository.Save(person);
            savedPerson.PersonId.ShouldNotBe(0, "A new Id should be generated for the person");

            var searchedPerson = (await personRepository.SearchByNames(
                    $"{person.FirstName} {person.MiddleName} {person.LastName}"))
                .FirstOrDefault();

            searchedPerson.ShouldNotBeNull("The saved person should be searchable");
            searchedPerson.PersonId.ShouldBe(savedPerson.PersonId);

            searchedPerson.PersonId = 0;
            var savedPersonJson = JsonConvert.SerializeObject(searchedPerson);
            var originalPersonJson = JsonConvert.SerializeObject(person);
            savedPersonJson.ShouldBe(originalPersonJson, "The person should have all it's properties saved");
        }
    }
}
