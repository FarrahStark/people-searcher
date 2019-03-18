using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace PeopleSearch.Tests.PersonTests
{
    public class SerializationTests
    {
        [Fact]
        public void Person_can_be_serialized()
        {
            var person = DataGenerator.GetPerson();
            Should.NotThrow(() => JsonConvert.SerializeObject(person));
        }

        [Fact]
        public void Person_can_be_deserialized()
        {
            var person = DataGenerator.GetPerson();
            var json = JsonConvert.SerializeObject(person);
            Should.NotThrow(() => JsonConvert.DeserializeObject<Person>(json));
        }
    }
}
