using Domain.Core.Models;

namespace InfrastructureTests.Data
{
    public class Test:AggregateRoot<int>
    {
        public Test(int id) : base(id)
        {

        }

        public Test(string name, DateTime dateTime, ICollection<TestInfo>? testInfos,int id):base(id)
        {
            Name = name;
            DateTime = dateTime;
            TestInfos = testInfos;
        }

        public string Name { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<TestInfo>? TestInfos { get; set; }
    }
}
