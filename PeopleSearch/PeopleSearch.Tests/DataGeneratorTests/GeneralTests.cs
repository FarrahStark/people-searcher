using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PeopleSearch.Tests.DataGeneratorTests
{
    public class GeneralTests
    {
        DataGenerator DataGenerator = new DataGenerator();

        [Fact]
        public void Random_returns_the_same_instance_the_second_time()
        {
            var expected = DataGenerator.Random;
            var actual = DataGenerator.Random;
            (expected == actual).ShouldBeTrue();
        }

        [Fact]
        public void NextPersonId_Gets_the_next_sequential_id()
        {
            for (int i = 0; i < 10; ++i)
            {
                var expected = DataGenerator.NextPersonId + 1;
                var actual = DataGenerator.NextPersonId;
                expected.ShouldBe(actual);
            }
        }

        [Fact]
        public void GetPerson_all_three_name_components_are_different()
        {
            //run 100 times to give reasonable certainty it will allways work
            for (int i = 0; i < 100; ++i)
            {
                var person = DataGenerator.GetPerson();
                var areNamesDifferent = person.FirstName != person.LastName &&
                person.FirstName != person.MiddleName &&
                person.LastName != person.MiddleName;

                areNamesDifferent.ShouldBeTrue("First last and middle names shouldn't be the same for any one person");
            }
        }

        [Fact]
        public void Shuffle_returns_a_new_array()
        {
            var original = GetPopulatedStringArray();
            var shuffled = original.Shuffle();
            (shuffled == original).ShouldBeFalse();
        }

        [Fact]
        public void Shuffle_does_not_modify_the_original_array()
        {
            var original = new[] {1, 2, 3, 4};
            var originalCopy = new[] { 1, 2, 3, 4 };
            var shuffled = original.Shuffle();
            for (int i = 0; i < original.Length; ++i)
            {
                original[i].ShouldBe(originalCopy[i]);
            }
        }

        [Fact]
        public void Shuffle_returns_the_same_set()
        {
            var original = GetPopulatedStringArray();
            var shuffled = original.Shuffle();

            shuffled.ShouldAllBe(s => original.Contains(s));
        }

        [Fact]
        public void Shuffle_returns_a_collection_of_the_same_length()
        {
            var original = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var shuffled = original.Shuffle().Distinct().ToArray();
            shuffled.Length.ShouldBe(original.Length);
        }

        private string[] GetPopulatedStringArray()
        {
            return MockData.MiddleNames;
        }
    }
}
