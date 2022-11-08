using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObject
{
    [Owned]
    public record Address : ValueObject<Address>
    {
        [StringLength(255)]
        public string Street { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string State { get; set; }


        public Address(string state,string country,string city,string street)
        {
            State = state;
            Country = country;
            City = city;
            Street = street;
        }
    }
}
