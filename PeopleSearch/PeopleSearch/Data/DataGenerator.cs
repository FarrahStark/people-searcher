using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PeopleSearch
{
    public class DataGenerator
    {
        public const int InitialId = 0;
        private static Lazy<Random> _random = new Lazy<Random>(() => new Random());
        public static Random Random => _random.Value;

        private static long _nextPersonId = InitialId;
        public long NextPersonId
        {
            get
            {
                Interlocked.Increment(ref _nextPersonId);
                return _nextPersonId;
            }
        }

        private static long _nextAddressId = InitialId;
        public long NextAddressId
        {
            get
            {
                Interlocked.Increment(ref _nextAddressId);
                return _nextAddressId;
            }
        }

        public IEnumerable<Person> GetPeople(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                yield return GetPerson();
            }
        }

        public (IList<Person> People, IList<Address> Addresses) GetPeopleAndAdresses(int count)
        {
            var people = new List<Person>(count);
            var addresses = new List<Address>(count);
            for (int i = 0; i < count; ++i)
            {
                var person = GetPerson(ignoreAddress: true);
                var address = GetAddress();
                address.PersonId = person.PersonId;
                people.Add(person);
                addresses.Add(address);
            }
            return (people, addresses);
        }

        public Person GetPerson(bool ignoreAddress = false)
        {
            var person = new Person
            {
                PersonId = NextPersonId,
                FirstName = MockData.FirstNames.RandomItem(),
                LastName = MockData.LastNames.RandomItem(),
                DateOfBirthUtc = GetDateInPast(),
                Interests = GetInterests(),
                ProfileImage = MockData.ProfileImages.RandomItem()
            };
            person.MiddleName = GetMiddleName(person.FirstName, person.LastName);
            if (!ignoreAddress)
            {
                person.Address = GetAddress();
                person.Address.Person = person;
                person.Address.PersonId = person.PersonId;
                person.Address.Person = person;
            }
            return person;
        }

        public Address GetAddress()
        {
            return new Address()
            {
                AddressId = NextAddressId,
                Line1 = GetStreetAddress(),
                Line2 = GetAddressLine2(),
                City = MockData.Cities.RandomItem(),
                State = MockData.StateCodes.RandomItem(),
                PostalCode = GetPostalCode(),
                Country = "United States"
            };
        }

        public string GetPostalCode()
        {
            return Random.Next(11111, 98111).ToString();
        }

        public string GetAddressLine2()
        {
            if (Random.Choice() && Random.Choice())
            {
                return $"{MockData.UnitDesignations.RandomItem()} {Random.Next(1, 150)}";
            }

            return string.Empty;
        }

        public string GetStreetAddress()
        {
            var streetComponents = new List<string>();
            streetComponents.Add(Random.Next(100, 9999).ToString());

            if (Random.Choice())
            {
                streetComponents.Add(MockData.Directions.RandomItem());
            }

            streetComponents.Add(MockData.StreetNames.RandomItem());
            return string.Join(" ", streetComponents);
        }

        public DateTime GetDateInPast()
        {
            const int centuryInDays = 36525;
            var ageSpan = TimeSpan.FromDays(Random.Next(1, centuryInDays));
            return DateTime.Today.ToUniversalTime() - ageSpan;
        }

        public string GetMiddleName(string firstName, string lastName)
        {
            string middleName;
            do
            {
                middleName = MockData.MiddleNames.RandomItem();
            } while (middleName == firstName || middleName == lastName);

            return middleName;
        }

        public string[] GetInterests()
        {
            return MockData.Interests.RandomSubset();
        }
    }
}
