using System.Linq.Dynamic.Core;

using Application.Core.DTO;

using Application.Core.ApplicationServices;

using Domain.Core.Repository;
using Domain.Entities;
using Infrastructure.Core.Repository.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationServices;

public class UserService : BaseService<User, int>, IUserService
{
    public UserService(IEfCoreRepositoryAsync<User, int> userRep, IUnitOfWork unitOfWork) : base(userRep, unitOfWork)
    {
    }

    protected override string[] Table => new[]{"UserInfo"};

    public PagedResult<User> GetUserAndInfo(SearchParams search, CancellationToken cancellationToken = default)
    {
        /*
         *  前提是遵循EFCore框架的导航属性设计
            Include会自动生成 LEFT JOIN语句
         */

        var list = BaseRep.GetQueryInclude(
             x => 
             x.Include(i => i.UserInfo)
             .Include(z=>z.Pets)
             .Include(y=>y.Cars))
            .PageResult(search.Page,search.PageSize);

        return list;
    }
}
