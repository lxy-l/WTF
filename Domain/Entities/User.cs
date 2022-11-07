﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Domain.AggregateRoots;
using Domain.ValueObject;

namespace Domain.Entities
{
    public class User: Entity<int>, IAggregateRoot
    {
        [Required]
        [StringLength(255)]
        public string? Name { get; set; }

        public DateTimeOffset Birthday { get; set; }

        public Address Address { get; private set; }

        public User(int id = default) : base(id)
        {
            //Address = new Address("","","","");
        }

        [JsonConstructor]
        public User(string? name, DateTimeOffset birthday,Address address, int id = default) : base(id)
        {
            Name = name;
            Birthday = birthday;
            Address = address;
        }

        /// <summary>
        /// 编辑信息
        /// </summary>
        /// <param name="user"></param>
        public void Edit(User user)
        {
            Name = user.Name;
            Birthday = user.Birthday;
            ModifyTime = DateTimeOffset.Now;
            Address = user.Address;
        }


    }
}
