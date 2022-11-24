using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests.Data
{
    public class TestInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Test Test { get; set; }
    }
}
