using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PeopleSearch
{
    public static class DataGenerator
    {
        public static Random Random { get { throw new NotImplementedException(); } }
        public static int NextPersonId { get { throw new NotImplementedException(); } }

        public static IEnumerable<Person> GetPeople(int count = 5)
        {
            throw new NotImplementedException();
        }

        public static Person GetPerson()
        {
            throw new NotImplementedException();
        }

        public static Address GetAddress()
        {
            throw new NotImplementedException();
        }

        public static string GetPostalCode()
        {
            throw new NotImplementedException();
        }

        public static string GetAddressLine2()
        {
            throw new NotImplementedException();
        }

        public static string GetStreetAddress()
        {
            throw new NotImplementedException();
        }

        public static bool Choice(this Random random)
        {
            throw new NotImplementedException();
        }

        public static T RandomItem<T>(this T[] items)
        {
            throw new NotImplementedException();
        }

        public static T[] RandomSubset<T>(this T[] items)
        {
            throw new NotImplementedException();
        }

        public static T[] Shuffle<T>(this IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public static DateTime GetDateInPast()
        {
            throw new NotImplementedException();
        }

        public static string GetMiddleName(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public static string[] GetInterests()
        {
            throw new NotImplementedException();
        }
    }
}
