using System.ComponentModel.DataAnnotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Core.Models.Tests
{
    [TestClass()]
    public class ValueObjectTests
    {
        [TestMethod()]
        public void EqualsTest()
        {
            Address address1 = new("A", "A", "B");
            Address address2 = new("A", "A", "B");
            Address address3 = new("A", "A", "A");

            Assert.IsTrue(EqualityComparer<Address>.Default.Equals(address2, address1));
            Assert.IsFalse(EqualityComparer<Address>.Default.Equals(address2, address3));

            Assert.IsTrue(Equals(address2, address1));
            Assert.IsFalse(Equals(address2, address3));


            Assert.IsTrue(address2.Equals(address1));
            Assert.IsFalse(address2.Equals(address3));

            Assert.IsTrue(address2 == address1);
            Assert.IsFalse(address2 != address1);
            Assert.IsTrue(address2 != address3);


            Assert.IsTrue(address2.GetHashCode() == address1.GetHashCode());
            Assert.IsFalse(address2.GetHashCode() != address1.GetHashCode());


        }

        [TestMethod()]
        public void GetCopyTest()
        {
            Address address1 = new("A", "A", "B");
            Address address3 = new("A", "A", "A");
            var address4 = address1.GetCopy();
            Assert.IsTrue(address4 == address1);
            Assert.IsTrue(address4 != address3);
        }
    }

    /// <summary>
    /// 地址
    /// </summary>
    class Address : ValueObject
    {
        /// <summary>
        /// 街道
        /// </summary>
        [StringLength(255)]
        public string Street { get; set; }


        /// <summary>
        /// 城市
        /// </summary>
        [StringLength(255)]
        public string City { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [StringLength(255)]
        public string Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <param name="street"></param>
        public Address(string country, string city, string street)
        {
            Country = country;
            City = city;
            Street = street;
        }

        /// <summary>
        /// 可以更加细化控制值比较
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
            yield return Street;
        }
    }
}