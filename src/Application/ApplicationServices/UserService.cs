using System.Linq.Dynamic.Core;

using Application.Core.DTO;

using Application.Core.ApplicationServices;

using Domain.Core.Repository;
using Domain.Entities;

using Infrastructure.Core.Repository;

using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationServices;

public class UserService : BaseService<User, int>, IUserService
{
    public UserService(IEFCoreRepositoryAsync<User, int> userRep, IUnitOfWork unitOfWork) : base(userRep, unitOfWork)
    {
    }

    public override string[]? Table => new string[]{"UserInfo"};

    public async Task<PagedResult<User>> GetUserAndInfo(SearchParams search, CancellationToken cancellationToken = default)
    {
        /*
         *  前提是遵循EFCore框架的导航属性设计
            Include会自动生成 LEFT JOIN语句
         */

        throw new Exception("1");
        var list = (await BaseRep.GetQueryIncludeAsync(
             x => 
             x.Include(i => i.UserInfo)
             .Include(x=>x.Pets)
             .Include(x=>x.Cars)))
            .PageResult(search.Page,search.PageSize);

        return list;
    }
}
