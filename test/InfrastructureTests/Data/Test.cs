using Domain.AggregateRoots;

namespace InfrastructureTests.Data
{
    public class Test:AggregateRoot<int>
    {
        public Test(int id) : base(id)
        {
        }

        public string Name { get; set; }


        public DateTime DateTime { get; set; }

        public virtual ICollection<TestInfo>? TestInfos { get; set; }
    }
}
