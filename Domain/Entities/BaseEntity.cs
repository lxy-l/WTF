using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    [Index(nameof(CreateTime),IsUnique =false)]
    [Index(nameof(ModifyTime),IsUnique =false)]
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
