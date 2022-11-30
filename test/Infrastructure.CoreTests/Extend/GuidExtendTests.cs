using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Core.Extend.Tests
{
    [TestClass()]
    public class GuidExtendTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            var guid1= GuidExtend.Create();
            Assert.IsNotNull(guid1);
        }
    }
}