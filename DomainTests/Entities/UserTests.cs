using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ValueObject;

namespace Domain.Entities.Tests
{
    /// <summary>
    /// 模型单元测试
    /// </summary>
    [TestClass()]
    public class UserTests
    {
        [TestMethod()]
        public void UserTest()
        {
            User user = 
                new User("1", DateTimeOffset.Now, 
                new Address("A", "B", "C", "D"),1);
            User user2 =
                new User("2", DateTimeOffset.Now,
                new Address("A", "B", "C", "D"),2);

            User user3 =
                new User("3", DateTimeOffset.Now,
                new Address("A", "B", "C", "E"), 3);

            Assert.IsFalse(user.Equals(user2));
            Assert.IsTrue(user2.Address.Equals(user.Address));
           

            Assert.IsFalse(user==user2);
            Assert.IsTrue(user.Address==user2.Address);
            Assert.IsTrue(user2 != user);

            Assert.IsFalse(user3.Address==user2.Address);
            Assert.IsFalse(user3.Address.Equals(user2.Address));
            Assert.IsTrue(user3.Address != user2.Address);
        }

        [TestMethod()]
        public void UserTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }
    }
}