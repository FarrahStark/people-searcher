using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PeopleSearch
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurePerson(modelBuilder.Entity<Person>().ToTable("Person"));
            ConfigureAddress(modelBuilder.Entity<Address>().ToTable("Address"));
        }

        private void ConfigurePerson(EntityTypeBuilder<Person> person)
        {
            person.HasKey(p => p.PersonId);
            person.Ignore(p => p.Age);
            person.Ignore(p => p.Interests);
            person.HasOne(p => p.Address)
                .WithOne(address => address.Person)
                .HasForeignKey<Address>(address => address.PersonId);
        }

        private void ConfigureAddress(EntityTypeBuilder<Address> address)
        {
            address.HasKey(a => a.AddressId);
            address.Property(a => a.State).HasMaxLength(2);
        }
    }
}
