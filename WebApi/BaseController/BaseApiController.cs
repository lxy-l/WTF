using System.Linq.Dynamic.Core;

using Application.ApplicationServices;
using Application.DTO;

using Domain.AggregateRoots;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.BaseController
{
    /// <summary>
    /// 通用控制器
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BaseApiController<TEntity,TKey> : ControllerBase where TEntity : Entity<TKey>, IAggregateRoot
    {
        /// <summary>
        /// 通用泛型服务
        /// </summary>
        protected readonly IBaseService<TEntity,TKey> _service;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="service"></param>
        public BaseApiController(IBaseService<TEntity, TKey> service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResult<TEntity>> Get([FromQuery] SeachParams seachParams)
        {
            var list = await _service.GetPagedResult(seachParams);
            return list;
        }

        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("GetModelById")]
        public async Task<TEntity?> GetModelById(TKey id)
        {
            return await _service.GetModelById(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">模型参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(TEntity model)
        {
            var user = await _service.AddEntity(model);
            return Ok(user);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">模型参数</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(TEntity model)
        {
            return Ok(await _service.EditEntity(model));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(TKey id)
        {
            return Ok(await _service.DeleteEntity(id));
        }
    }
}
