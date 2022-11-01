using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public DateTimeOffset ModifyTime { get; set; }

        public BaseEntity()
        {
            CreateTime = DateTimeOffset.Now;
            ModifyTime = DateTimeOffset.Now;
        }
    }
}
