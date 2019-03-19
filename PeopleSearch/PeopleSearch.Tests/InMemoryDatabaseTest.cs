using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleSearch.Tests
{
    public class InMemoryDatabaseTest
    {
        protected static DataGenerator DataGenerator = new DataGenerator();
        protected readonly PersonRepository PersonRepository =
            new PersonRepository(
                new DbContextOptionsBuilder<PersonContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options,
                DataGenerator,
                new PeopleSearchSettings());
    }
}
