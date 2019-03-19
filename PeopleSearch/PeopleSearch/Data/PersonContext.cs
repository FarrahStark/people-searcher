using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace PeopleSearch
{
    public class PersonContext : DbContext
    {
        private readonly DataGenerator dataGenerator;
        private readonly PeopleSearchSettings settings;

        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public PersonContext(
            DbContextOptions<PersonContext> options,
            DataGenerator dataGenerator,
            PeopleSearchSettings settings) : base(options)
        {
            this.dataGenerator = dataGenerator;
            this.settings = settings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var data = dataGenerator.GetPeopleAndAdresses(settings.DataGeneratorPersonSeedCount);
            ConfigurePerson(modelBuilder.Entity<Person>().ToTable("Person"), data.People.ToArray());
            ConfigureAddress(modelBuilder.Entity<Address>().ToTable("Address"), data.Addresses.ToArray());
        }

        private void ConfigurePerson(EntityTypeBuilder<Person> person, params object[] data)
        {
            person.HasKey(p => p.PersonId);
            person.Ignore(p => p.Age);
            person.Ignore(p => p.Interests);
            person.HasOne(p => p.Address)
                .WithOne(address => address.Person)
                .HasForeignKey<Address>(address => address.PersonId);
            if (data.Any())
            {
                person.HasData(data);
            }
        }

        private void ConfigureAddress(EntityTypeBuilder<Address> address, params object[] data)
        {
            address.HasKey(a => a.AddressId);
            address.Property(a => a.State).HasMaxLength(2);
            if (data.Any())
            {
                address.HasData(data);
            }
        }
    }
}
