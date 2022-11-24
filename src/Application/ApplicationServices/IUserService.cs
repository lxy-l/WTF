using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using Application.DTO;

using Domain.Entities;

namespace Application.ApplicationServices
{
    /// <summary>
    /// 自定义用户信息查询
    /// </summary>
    public interface IUserService:IBaseService<User,int>
    {
        /// <summary>
        /// 获取全部用户包含用户信息
        /// </summary>
        /// <returns></returns>
        Task<PagedResult<User>> GetUserAndInfo(SearchParams search, CancellationToken cancellationToken = default);
    }
}
