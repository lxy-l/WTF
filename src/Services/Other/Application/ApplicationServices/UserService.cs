using Crafty.Application.Core.ApplicationServices;
using Crafty.Domain.Core.Repository;
using Crafty.Infrastructure.EFCore.Repository;
using Crafty.Infrastructure.EFCore.UnitOfWork;

using Domain.Entities;

using Infrastructure.Context;

namespace Application.ApplicationServices;

public class UserService : BaseService<User,int>,IUserService
{
    public UserService(IUnitOfWork<UserDbContext> unitOfWork, IEfCoreRepository<UserDbContext,User> baseRep) : base(unitOfWork, baseRep)
    {
    }
    protected override string[] Table => new[] { "UserInfo" };
}
