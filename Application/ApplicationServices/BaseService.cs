using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.AggregateRoots;
using Domain.Entities;
using Domain.Repository;

namespace Application.ApplicationServices
{
    public class BaseService<TEntity,TKey> : IBaseService<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IUserRepositoryAsync<TEntity, UserViewModel, TKey> _baseRep;

        public BaseService(IUserRepositoryAsync<TEntity, UserViewModel, TKey> userRep, IUnitOfWork unitOfWork)
        {
            _baseRep = userRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<TEntity> AddEntity(TEntity model)
        {
            await _baseRep.InsertAsync(model);
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<TEntity> DeleteEntity(TKey id)
        {
            TEntity? model = await _baseRep.FindByIdAsync(id);
            if (model is null)
            {
                throw new Exception("未找到实体信息！");
            }
            _baseRep.Delete(model);
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<TEntity> EditEntity(TEntity model)
        {
            _baseRep.Update(model);
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<PagedResult<TEntity>> GetPagedResult(SeachParams seachParams)
        {
            return await _baseRep.GetPagedResultAsync(page: seachParams.Page, pageSize: seachParams.PageSize);
        }
    }
}
