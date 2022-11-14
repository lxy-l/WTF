using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.ValueObject;

namespace Domain.Entities
{
    public class UserInfo:Entity<int>
    {
        public UserInfo(int id) : base(id)
        {
        }

        [Required]
        [StringLength(55)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string CardId { get; set; }

        [Required]
        public Address Address { get; set; }
    }
}
