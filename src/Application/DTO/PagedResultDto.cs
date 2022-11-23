using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PagedResultDto<TEntity>
    {
        public PagedResultDto() { }

        public int Page { get; set; }

        public int Total { get; set; }

        public virtual IEnumerable<TEntity> List { get; set; }
    }
}
