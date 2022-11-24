using Domain.Entities;

namespace InfrastructureTests.Data
{
    public class TestInfo:Entity<int>
    {
        public TestInfo(int id,string name) : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }

    }
}
