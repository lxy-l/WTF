using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.Entities;
using Domain.Repository;

namespace Application.ApplicationServices
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService:BaseService<User,int>, IUserService<User,int>
    {
        public UserService(IUserRepositoryAsync<User, UserViewModel, int> userRep, IUnitOfWork unitOfWork) : base(userRep, unitOfWork)
        {
        }

        public async Task BulkInsertUser(List<User> users)
        {
            await _baseRep.BatchInsertAsync(users);
            await _unitOfWork.BulkCommitAsync(new EFCore.BulkExtensions.BulkConfig { BatchSize=50000});
        }
    }
}
