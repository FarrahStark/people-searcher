using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Moq;
using Shouldly;

namespace PeopleSearch.Core.Tests.PersonTests
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
