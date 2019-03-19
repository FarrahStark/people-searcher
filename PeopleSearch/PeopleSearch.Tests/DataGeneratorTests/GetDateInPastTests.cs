using System;
using Shouldly;
using System.Linq;
using Xunit;

namespace PeopleSearch.Tests.DataGeneratorTests
{
    public class GetDateInPast
    {
        DataGenerator DataGenerator = new DataGenerator();

        [Fact]
        public void Returns_a_past_date()
        {
            for (int i = 0; i < 50; ++i)
            {
                var actual = DataGenerator.GetDateInPast();
                (DateTime.Now.ToUniversalTime() > actual).ShouldBeTrue();
            }
        }

        [Fact]
        public void Returns_a_utc_date()
        {
            for (int i = 0; i < 50; ++i)
            {
                var actual = DataGenerator.GetDateInPast();
                (actual.Kind == DateTimeKind.Utc).ShouldBeTrue();
            }
        }
    }
}
