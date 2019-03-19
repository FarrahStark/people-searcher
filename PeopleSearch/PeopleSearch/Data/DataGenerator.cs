using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PeopleSearch
{
    public static class DataGenerator
    {
        public const int InitialId = 0;
        private static Lazy<Random> _random = new Lazy<Random>(() => new Random());
        public static Random Random => _random.Value;

        private static long _nextPersonId = InitialId;
        public static long NextPersonId => _nextPersonId++;

        public static IEnumerable<Person> GetPeople(int count = 5)
        {
            for (int i = 0; i < count; ++i)
            {
                yield return GetPerson();
            }
        }

        public static Person GetPerson()
        {
            var person = new Person
            {
                FirstName = MockData.FirstNames.RandomItem(),
                LastName = MockData.LastNames.RandomItem(),
                DateOfBirthUtc = GetDateInPast(),
                Interests = GetInterests(),
                ProfileImage = MockData.ProfileImages.RandomItem()
            };
            person.MiddleName = GetMiddleName(person.FirstName, person.LastName);
            person.Address = GetAddress();
            person.Address.Person = person;
            return person;
        }

        public static Address GetAddress()
        {
            return new Address()
            {
                Line1 = GetStreetAddress(),
                Line2 = GetAddressLine2(),
                City = MockData.Cities.RandomItem(),
                State = MockData.StateCodes.RandomItem(),
                PostalCode = GetPostalCode(),
                Country = "United States"
            };
        }

        public static string GetPostalCode()
        {
            return Random.Next(11111, 98111).ToString();
        }

        public static string GetAddressLine2()
        {
            if (Random.Choice() && Random.Choice())
            {
                return $"{MockData.UnitDesignations.RandomItem()} {Random.Next(1, 150)}";
            }

            return string.Empty;
        }

        public static string GetStreetAddress()
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

        public static bool Choice(this Random random)
        {
            return random.Next(0, 2) == 0;
        }

        public static T RandomItem<T>(this T[] items)
        {
            var index = Random.Next(0, items.Length);
            return items[index];
        }

        public static T[] RandomSubset<T>(this T[] items)
        {
            int count = Random.Next(0, items.Length);
            return items.Shuffle().Take(count).ToArray();
        }

        public static T[] Shuffle<T>(this IEnumerable<T> items)
        {
            var itemsArray = items.ToArray();
            var shuffled = new T[itemsArray.Length];
            Array.Copy(itemsArray, shuffled, itemsArray.Length);
            for (int i = 0; i < itemsArray.Length; ++i)
            {
                var j = Random.Next(i, itemsArray.Length);
                var temp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = temp;
            }

            return shuffled;
        }

        public static DateTime GetDateInPast()
        {
            const int centuryInDays = 36525;
            var ageSpan = TimeSpan.FromDays(Random.Next(1, centuryInDays));
            return DateTime.Today.ToUniversalTime() - ageSpan;
        }

        public static string GetMiddleName(string firstName, string lastName)
        {
            string middleName;
            do
            {
                middleName = MockData.MiddleNames.RandomItem();
            } while (middleName == firstName || middleName == lastName);

            return middleName;
        }

        public static string[] GetInterests()
        {
            return MockData.Interests.RandomSubset();
        }
    }
}
