using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject
{
    public class Address : ValueObject<Address>
    {
        [StringLength(255)]
        public string Street { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string State { get; set; }


        public Address()
        {

        }

        public Address(string state,string country,string city,string street)
        {
            State = state;
            Country = country;
            City = city;
            Street = street;
        }
    }
}
