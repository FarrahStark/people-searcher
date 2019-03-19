using Newtonsoft.Json;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PeopleSearch.Tests.PersonRepositoryTests
{
    public class SaveTests : InMemoryDatabaseTest
    {
        [Fact]
        public async Task When_save_is_called_the_person_is_persisted_with_an_id()
        {
            var person = DataGenerator.GetPerson();
            person.PersonId = 0;
            person.Address.AddressId = 0;
            person.Address.PersonId = 0;
            var originalPersonJson = JsonConvert.SerializeObject(person);
            var savedPerson = await PersonRepository.Save(person);
            savedPerson.PersonId.ShouldNotBe(0, "A new Id should be generated for the person");

            var searchedPerson = (await PersonRepository.SearchByNames(
                    $"{person.FirstName} {person.MiddleName} {person.LastName}"))
                .FirstOrDefault();

            searchedPerson.ShouldNotBeNull("The saved person should be searchable");
            searchedPerson.PersonId.ShouldBe(savedPerson.PersonId);

            ///set these properties to pre-insertion values so the json compare is accurate
            searchedPerson.PersonId = 0;
            searchedPerson.Address.AddressId = 0;
            searchedPerson.Address.PersonId = 0;
            var savedPersonJson = JsonConvert.SerializeObject(searchedPerson);
            savedPersonJson.ShouldBe(originalPersonJson, "The person should have all it's properties saved");
        }
    }
}
