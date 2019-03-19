using System;
using System.Collections.Generic;
using System.Linq;

namespace PeopleSearch
{
    public static class DataHelpers
    {
        public static bool Choice(this Random random)
        {
            return random.Next(0, 2) == 0;
        }

        public static T RandomItem<T>(this T[] items)
        {
            var index = DataGenerator.Random.Next(0, items.Length);
            return items[index];
        }

        public static T[] RandomSubset<T>(this T[] items)
        {
            int count = DataGenerator.Random.Next(0, items.Length);
            return items.Shuffle().Take(count).ToArray();
        }

        public static T[] Shuffle<T>(this IEnumerable<T> items)
        {
            var itemsArray = items.ToArray();
            var shuffled = new T[itemsArray.Length];
            Array.Copy(itemsArray, shuffled, itemsArray.Length);
            for (int i = 0; i < itemsArray.Length; ++i)
            {
                var j = DataGenerator.Random.Next(i, itemsArray.Length);
                var temp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = temp;
            }

            return shuffled;
        }
    }
}
