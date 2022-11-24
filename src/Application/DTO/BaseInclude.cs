using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public abstract class BaseInclude
    {
        public virtual List<string>? Table { get; }
    }
}
