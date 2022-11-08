using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq.Dynamic.Core;

using Application.ApplicationServices;
using Application.DTO;

using Domain.Entities;
using Domain.ValueObject;

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
        public async Task<PagedResult<User>> Get([FromQuery]SeachParams seachParams)
        {
            return await _userService.GetUsers(seachParams);
        }

        /// <summary>
        /// 新增一千万用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertList")]
        public async Task<IActionResult> Add()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<User> users = Enumerable.Range(1, 1000_0000)
                .Select(index => 
                new User("Name_" + index, DateTimeOffset.Now, new Address("未知", "未知", "未知", "未知"),index))
                .ToList();
            stopwatch.Stop();
            long Time = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            await _userService.BulkInsertUser(users);
            stopwatch.Stop();
            long DataBaseTime = stopwatch.ElapsedMilliseconds;
            return Ok(new { Time,DataBaseTime});
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(User model)
        {
            var user = await _userService.AddUser(model);
            return Ok(user);
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
