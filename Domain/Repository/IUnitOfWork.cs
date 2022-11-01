using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// 提交更改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> Commit(CancellationToken cancellationToken = default);


        /// <summary>
        /// 回滚更改
        /// </summary>
        Task RollBack();
    }
}
