using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    public class User: BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string? Name { get; set; }

        public DateTimeOffset Birthday { get; set; }

        public User(string? name, DateTimeOffset birthday)
        {
            Name = name;
            Birthday = birthday;
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
        }


    }
}
