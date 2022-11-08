using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SeachParams
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Required]
        public int PageSize { get; set; } = 20;
    }
}
