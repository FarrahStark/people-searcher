using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PeopleSearch
{
    public class Person
    {
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirthUtc { get; set; }
        public Address Address { get; set; }
        public int Age
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<string> Interests { get; set; }

        [JsonIgnore]
        public virtual DateTime UtcToday => DateTime.Today.ToUniversalTime();

        [JsonIgnore]
        public virtual string InterestsJson
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }
    }
}
