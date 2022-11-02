using Application.ApplicationServices;

using Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userService.GetUsers();
        }

        /// <summary>
        /// 新增一千万用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertList")]
        public async Task<IActionResult> Add()
        {
            await _userService.InsertRangAsync();
            return Ok();
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            await _userService.AddUser(user);
            return Ok();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(User user)
        {
            return Ok(await _userService.EditUser(user));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _userService.DeleteUser(id));
        }
    }
}
