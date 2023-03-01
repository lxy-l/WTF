using System.Linq.Dynamic.Core;

using Application.Core.ApplicationServices;
using Application.Core.DTO;

using Domain.Core.Repository;
using Domain.Entities;

using Infrastructure.Core.Repository.EFCore;

using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationServices;

public class UserService : IUserService
{
    IEfCoreRepositoryAsync<User, int> _userRep { get; }

    public UserService(IEfCoreRepositoryAsync<User, int> userRep)
    {
        _userRep = userRep;
    }

    ////TODO 考虑更好的解决方案
    //protected override string[] Table => new[]{"UserInfo"};


    public PagedResult<User> GetUserAndInfo(SearchParams search, CancellationToken cancellationToken = default)
    {
        /*
         *  前提是遵循EFCore框架的导航属性设计
            Include会自动生成 LEFT JOIN语句
         */

        var list = _userRep.GetQueryInclude(
             x => 
             x.Include(i => i.UserInfo)
             .Include(z=>z.Pets)
             .Include(y=>y.Cars))
            .PageResult(search.Page,search.PageSize);

        return list;
    }
}
