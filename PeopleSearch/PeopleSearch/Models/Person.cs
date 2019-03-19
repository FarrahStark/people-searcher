using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleSearch
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
                var today = UtcToday;
                var age = today.Year - DateOfBirthUtc.Year;
                if (DateOfBirthUtc > today.AddYears(-1 * age))
                {
                    --age;
                }

                return age;
            }
        }

        public IEnumerable<string> Interests { get; set; }

        [JsonIgnore]
        public virtual DateTime UtcToday => DateTime.Today.ToUniversalTime();

        [JsonIgnore]
        public virtual string InterestsJson
        {
            get { return JsonConvert.SerializeObject(Interests ?? new string[0]); }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Interests = new string[0];
                }
                else
                {
                    Interests = JsonConvert.DeserializeObject<List<string>>(value);
                }
            }
        }
    }
}
