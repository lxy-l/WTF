using Infrastructure.Repository;
using Domain.Entities;
using Domain.Repository;

using Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Repository.Tests
{
    [TestClass]
    public class RepositoryAsyncTests
    {
        static UserDbContext dbContext = new(new DbContextOptions<UserDbContext>());

        IUnitOfWork unitOfWork = new UnitOfWork.UnitOfWork(dbContext);

        IRepositoryAsync<User, int> repository = new RepositoryAsync<User, int>(dbContext);



        [TestMethod()]
        public void SingleAsyncTest1()
        {
            var user = repository.SingleAsync(x => x.Id == 1).Result;
            Assert.IsNotNull(user);
            Assert.ThrowsException<AggregateException>(() => { var a = repository.SingleAsync(x => x.Id == 0).Result; });
        }

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
            var user = repository.SingleAsync(x => x.Id == 1).Result;
            Assert.IsNotNull(user);
            user.Name = "修改后";
            repository.Update(user);
            unitOfWork.CommitAsync().Wait();
            var user2 = repository.FindByIdAsync(user.Id).Result;
            Assert.IsNotNull(user2);
            Assert.AreEqual(user.Name, user2.Name);

            var users = repository.GetPagedResultAsync(x => x.CreateTime < DateTimeOffset.Now, page: 1, pageSize: 5).Result;
            Assert.IsNotNull(users.Queryable);
            Assert.AreEqual(5, users.Queryable.Count());
            var list = users.Queryable.ToList();
            foreach (var item in list)
            {
                item.Name = "修改后Name";
                item.Address.Country = "中国";
            }
            repository.UpdateAsync(list).Wait();
            unitOfWork.BulkCommitAsync().Wait();
            var users2 = repository.GetQueryAsync(x => x.Name == "修改后Name" && x.Address.Country == "中国").Result;
            Assert.AreEqual(users2.Count, users.Queryable.Count());
        }

        [TestMethod()]
        public void DeleteAsyncTest()
        {
            var list = repository.GetQueryAsync(x => x.Address.City == "测试" && x.Name == "修改后Name").Result;
            Assert.IsNotNull(list);
            int count = repository.DeleteAsync(x => x.Address.City == "测试" && x.Name == "修改后Name").Result;
            Assert.AreEqual(list.Count, count);
            unitOfWork.BulkCommitAsync().Wait();
            long num = repository.CountAsync(x => x.Address.City == "测试" && x.Name == "修改后Name").Result;
            Assert.AreEqual(0, num);
        }

        [TestMethod()]
        public void DeleteAsyncTest1()
        {
            long count1 = repository.CountAsync().Result;
            var list = repository.GetQueryAsync(x => x.Name == "修改后Name").Result;
            Assert.IsNotNull(list);
            repository.DeleteAsync(list).Wait();
            unitOfWork.BulkCommitAsync().Wait();
            long count2 = repository.CountAsync().Result;
            Assert.AreEqual(count1 - list.Count, count2);
        }

        [TestMethod()]
        public void FirstOrDefaultAsyncTest()
        {
            var user = repository.FirstOrDefaultAsync().Result;
            Assert.IsNotNull(user);
            var user2 = repository.FirstOrDefaultAsync(x => x.Id == 500).Result;
            Assert.IsNotNull(user2);
        }

        [TestMethod()]
        public void GetQueryAsyncTest1()
        {
            var list = repository.GetQueryAsync(w=>w.Id<10,orderBy:x=>x.OrderBy(o=>o.CreateTime)).Result;
            Assert.IsNotNull(list);
            Assert.IsTrue(list[0].CreateTime < list[1].CreateTime);

            var list2 = repository.GetQueryAsync(w => w.Id < 10, orderBy: x => x.OrderByDescending(o => o.CreateTime)).Result;
            Assert.IsNotNull(list2);
            Assert.IsTrue(list2[0].CreateTime > list2[1].CreateTime);
        }

        [TestMethod()]
        public void GetPagedResultAsyncTest1()
        {
            var list = repository.GetPagedResultAsync( orderBy: x => x.OrderBy(o => o.CreateTime)).Result;
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Queryable.ToList()[0].CreateTime < list.Queryable.ToList()[1].CreateTime);

            var list2 = repository.GetPagedResultAsync(orderBy: x => x.OrderByDescending(o => o.CreateTime)).Result;
            Assert.IsNotNull(list2);
            Assert.IsTrue(list2.Queryable.ToList()[0].CreateTime > list2.Queryable.ToList()[1].CreateTime);
        }
    }
}