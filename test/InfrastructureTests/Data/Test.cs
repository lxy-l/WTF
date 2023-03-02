using Domain.Core.Models;

namespace InfrastructureTests.Data;

public class Test:Entity<int>,IAggregateRoot
{
    public Test(int id) : base(id)
    {

    }

    public Test(string name, DateTime dateTime, ICollection<TestInfo>? testInfos,int id):base(id)
    {
        Name = name;
        Time = dateTime;
        TestInfos = testInfos;
    }

    public string Name { get; set; }

    public DateTime Time { get; set; }

    public ICollection<TestInfo>? TestInfos { get; set; }
}