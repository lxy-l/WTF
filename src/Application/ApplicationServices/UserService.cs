using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using Application.DTO;

using Domain.Entities;
using Domain.Repository;

using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationServices
{
    public class UserService : BaseService<User, int>, IUserService
    {
        public UserService(IRepositoryAsync<User, int> userRep, IUnitOfWork unitOfWork) : base(userRep, unitOfWork)
        {
        }

        public async Task<PagedResult<User>> GetUserAndInfo(SearchParams search)
        {
            /*
             *  前提是遵循EFCore框架的导航属性设计
                Include会自动生成 LEFT JOIN语句 以BaseRep为主表查询从表信息
             */
            var list= await BaseRep.GetPagedResultAsync(
                include: x => x.Include(i => i.UserInfo),
                page: search.Page, 
                pageSize: search.PageSize);

            return list;
        }
    }
}
