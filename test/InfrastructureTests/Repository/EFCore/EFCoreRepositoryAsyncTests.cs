using System.Globalization;

using Domain.Entities;

using InfrastructureTests.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Infrastructure.Repository.Tests
{
    [TestClass()]
    public class EFCoreRepositoryAsyncTests
    {
        //~EFCoreRepositoryAsyncTests()
        //{
        //    DbContext.Dispose();
        //}
        //static DbContext DbContext = new TestDbContext();
        


        [TestMethod()]
        public void GetQueryAsyncTest()
        {
            using TestDbContext DbContext = new TestDbContext();
            DbContext.Tests.ToList();
            IEFCoreRepositoryAsync<Test, int> _repositoryAsync = new EFCoreRepositoryAsync<Test, int>(DbContext);
            var count=_repositoryAsync.CountAsync().Result;
            string exp =""" Name="string" && TestInfo !=null """;
            var list= _repositoryAsync.GetQueryAsync(exp).Result;
            exp = """ Name="string" && UserInfo ==null """;
            var list2 = _repositoryAsync.GetQueryAsync(exp).Result;
            Assert.IsNotNull(list);
            Assert.IsNotNull(list2);
            Assert.AreEqual(list2.Count()+list.Count(),count);
        }

        
    }
}