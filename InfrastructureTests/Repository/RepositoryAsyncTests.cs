using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Domain.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Tests
{
    [TestClass]
    public class RepositoryAsyncTests
    {
        static UserDbContext dbContext = new(new DbContextOptions<UserDbContext>());

        IUnitOfWork unitOfWork = new UnitOfWork.UnitOfWork(dbContext);

        IRepositoryAsync<User, int> repository = new RepositoryAsync<User, int>(dbContext);


        [TestMethod]
        public void GetQueryAsyncTest()
        {
            var list = repository.GetQueryAsync(x => x.Id == 1).Result;
            Assert.IsNotNull(list);
        }

        [TestMethod()]
        public void SingleAsyncTest()
        {
            var user = repository.SingleAsync(x => x.Id == 1).Result;
            Assert.IsNotNull(user);
        }

        [TestMethod()]
        public void GetPagedResultBySelectAsyncTest()
        {
            var user = repository.GetPagedResultBySelectAsync(x => new { x.Name, x.Birthday }, x => x.CreateTime <= DateTimeOffset.Now, page: 1, pageSize: 5).Result;
            Assert.IsNotNull(user.Queryable);
            Assert.IsTrue(user.Queryable.Count() == 5);
        }

        [TestMethod()]
        public void GetPagedResultAsyncTest()
        {
            var user = repository.GetPagedResultAsync(x => x.CreateTime < DateTimeOffset.Now).Result;
            Assert.IsNotNull(user.Queryable);
            Assert.IsTrue(user.Queryable.Count() == 20);
        }

        [TestMethod()]
        public void BatchInsertAsyncTest()
        {
             repository.BatchInsertAsync(new List<User>
            {
                new User("111",DateTimeOffset.Now,new Domain.ValueObject.Address("测试","测试","测试","测试")),
                 new User("222",DateTimeOffset.Now,new Domain.ValueObject.Address("测试","测试","测试","测试")),
                  new User("333",DateTimeOffset.Now,new Domain.ValueObject.Address("测试","测试","测试","测试"))
            }).Wait();
            unitOfWork.BulkCommitAsync().Wait();
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void UpdateAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAsyncTest1()
        {
            Assert.Fail();
        }
    }
}