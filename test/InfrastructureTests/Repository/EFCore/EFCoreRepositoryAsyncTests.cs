using Infrastructure.Core.Repository.EFCore;
using InfrastructureTests.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Repository.Tests;

[TestClass()]
public class EFCoreRepositoryAsyncTests:IDisposable
{
    private readonly TestDbContext DbContext = new();
    private bool disposedValue;

    public EFCoreRepositoryAsyncTests()
    {
        if (!DbContext.Tests.Any())
        {
            DbContext.AddRange(new List<Test>
            {
                new Test("string",DateTime.UtcNow,new List<TestInfo>{new TestInfo(1,"B1")},1),
                new Test("string",DateTime.UtcNow.AddDays(1),
                    new List<TestInfo>{new TestInfo(2,"B2")},2),
                new Test("string",DateTime.UtcNow.AddDays(2),null,3),
                new Test("string",DateTime.UtcNow.AddDays(3),null,4),
                new Test("string",DateTime.UtcNow.AddDays(4),null,5),
                new Test("string",DateTime.UtcNow.AddDays(5),null,6),
            });
            DbContext.SaveChanges();
        }  

    }

    [DataRow("""Name="string" && TestInfos.Count>0""", "DateTime DESC", """Name="string"&&TestInfos.Count=0""", "DateTime ASC")]
    [TestMethod()]
    public void GetDynamicQueryAsyncTest(string exp1,string sort1, string exp2,string sort2)
    {
        IEfCoreRepositoryAsync<Test, int> _repositoryAsync = new EfCoreRepositoryAsync<Test, int>(DbContext);
        var count = _repositoryAsync.CountAsync().Result;
        Assert.IsTrue(count != 0);
        var list = _repositoryAsync.GetDynamicQueryAsync(exp1,sort1).Result.ToList();
        var list2 = _repositoryAsync.GetDynamicQueryAsync(exp2,sort2).Result.ToList();
        Assert.AreEqual(2, list.Count);
        Assert.AreEqual(4, list2.Count);
        var before = list.ToList()[0];
        var after = list.ToList()[1];
        Assert.IsTrue(before.DateTime > after.DateTime);

        before = list2.ToList()[0];
        after = list2.ToList()[1];
        Assert.IsTrue(before.DateTime < after.DateTime);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
            }
            DbContext.Dispose();
            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~EFCoreRepositoryAsyncTests()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}