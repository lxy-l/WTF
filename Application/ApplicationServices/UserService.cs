using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.Entities;
using Domain.Repository;

namespace Application.ApplicationServices
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepositoryAsync<User,UserViewModel, int> _userRep;

        public UserService(IUserRepositoryAsync<User, UserViewModel, int> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<User>> GetUsers(SeachParams seachParams)
        {
            return await _userRep.GetPagedResultAsync(page:seachParams.Page,pageSize:seachParams.PageSize);
        }

        public async Task<User> AddUser(User user)
        {
            await _userRep.InsertAsync(user);
            await _unitOfWork.CommitAsync();
            return user;
        }

        public async Task<User> DeleteUser(int id)
        {
            User? user =await _userRep.FindByIdAsync(id);
            if (user is null)
            {
                throw new Exception("未找到实体信息！");
            }
            _userRep.Delete(user);
            await _unitOfWork.CommitAsync();
            return user;
        }

        public async Task<User> EditUser(User model)
        {
            User? user = await _userRep.FindByIdAsync(model.Id);
            if (user is null)
            {
                throw new Exception("未找到实体信息！");
            }
            //核心业务逻辑
            user.Edit(model);
            //持久化
            _userRep.Update(user);
            await _unitOfWork.CommitAsync();
            return user;
        }

        public async Task BulkInsertUser(List<User> users)
        {
            await _userRep.BatchInsertAsync(users);
            await _unitOfWork.BulkCommitAsync(new EFCore.BulkExtensions.BulkConfig { BatchSize=50000});
        }
    }
}
