using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleSearch
{
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long AddressId { get; set; }
        public virtual long PersonId { get; set; }
        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }
        [JsonIgnore]
        public virtual Person Person { get; set; }
    }
}
