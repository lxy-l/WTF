using System.Net;

using Domain.Entities;
using Domain.Repository;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase,IDisposable
    {

        public readonly IRepositoryAsync<User, int> _userRep;
        public readonly IUnitOfWork _unitOfWork;
        private bool disposedValue;

        public UsersController(IRepositoryAsync<User, int> userRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userRep.GetListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            await _userRep.InsertAsync(user);
            await _unitOfWork.Commit();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(User user)
        {
            var model = await _userRep.FindByIdAsync(user.Id);
            if (model is null)
            {
                return NotFound();
            }
            model.Name=user.Name;
            model.Birthday = user.Birthday;
            model.ModifyTime = DateTimeOffset.Now;

            await _userRep.UpdateAsync(model);
            await _unitOfWork.Commit();

            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _userRep.FindByIdAsync(id);
            if (model is null)
            {
                return NotFound();
            }

            await _userRep.DeleteAsync(model);
            return Ok(model);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   _userRep.Dispose();
                   _unitOfWork.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~UsersController()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
