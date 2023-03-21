using Crafty.Application.Core.ApplicationServices;
using Crafty.Application.Core.DTO;
using Crafty.Domain.Core.Models;
using Crafty.Domain.Core.Repository;
using Crafty.Domain.Core.UnitOfWork;

using Domain.Entities;

namespace Application.ApplicationServices;

public class UserService : BaseService<User,int>,IUserService
{
    public UserService(IUnitOfWork unitOfWork, IRepository<User> baseRep) : base(unitOfWork, baseRep)
    {
    }

    //private readonly IEfCoreRepository<User> _userRep;
    //private readonly IUnitOfWork _unitOfWork;

    //public UserService(IEfCoreRepository<User> userRep)
    //{
    //    _userRep = userRep;
    //}

    ////TODO 考虑更好的解决方案
    //protected override string[] Table => new[]{"UserInfo"};


    public Task<IPagedList<User>> GetUserAndInfo(SearchParams search, CancellationToken cancellationToken = default)
    {
        return null;
    }
}
