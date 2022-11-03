using System;

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
        private readonly IRepositoryAsync<User, int> _userRep;

        public UserService(IRepositoryAsync<User, int> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _userRep.GetListAsync();
        }

        public async Task<User> AddUser(User model)
        {
            await _userRep.InsertAsync(model);
            await _unitOfWork.Commit();
            return model;
        }

        public async Task<User> DeleteUser(int id)
        {
            User? user =await _userRep.FindByIdAsync(id);
            if (user is null)
            {
                throw new Exception("未找到实体信息！");
            }
            await _userRep.DeleteAsync(user);
            await _unitOfWork.Commit();
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
            await _userRep.UpdateAsync(user);
            await _unitOfWork.Commit();
            return model;
        }

        public async Task InsertRangAsync()
        {
            List<User> users = Enumerable.Range(1, 10000000)
                .Select(index => new User("Name_" + index, DateTimeOffset.Now))
                .ToList();

            await _userRep.InsertRangAsync(users);
            users.Clear();
        }
    }
}
