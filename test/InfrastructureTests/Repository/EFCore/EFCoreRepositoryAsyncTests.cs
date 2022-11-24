using InfrastructureTests.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Repository.Tests
{
    [TestClass()]
    public class EFCoreRepositoryAsyncTests
    {
        [DataRow( """ Name="string" && TestInfos !=null """, """ Name="string" && TestInfos ==null """ )]
        [TestMethod()]
        public void GetQueryAsyncTest(string exp1,string exp2)
        {
            using TestDbContext DbContext = new TestDbContext();
            DbContext.AddRange(new List<Test>
            {
                   new Test("string",DateTime.UtcNow,new List<TestInfo>{new TestInfo(1,"B1")},1),
                   new Test("string",DateTime.UtcNow,null,2),
                   new Test("string",DateTime.UtcNow,null,3),
                   new Test("string",DateTime.UtcNow,null,4),
                   new Test("string",DateTime.UtcNow,null,5),
                   new Test("string",DateTime.UtcNow,null,6),
            });
            DbContext.SaveChanges();
            var list3=DbContext.Tests.ToList();
            IEFCoreRepositoryAsync<Test, int> _repositoryAsync = new EFCoreRepositoryAsync<Test, int>(DbContext);
            var count=_repositoryAsync.CountAsync().Result;
            Assert.IsTrue(count!=0);
            var list= _repositoryAsync.GetQueryAsync(exp1).Result;
            var list2 = _repositoryAsync.GetQueryAsync(exp2).Result;
            Assert.IsNotNull(list);
            Assert.IsNotNull(list2);
            Assert.AreEqual(list2.Count()+list.Count(),count);
        }

        
    }
}