using System.Linq.Dynamic.Core;

using Application.ApplicationServices;
using Application.DTO;

using Domain.AggregateRoots;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.BaseController;

/// <summary>
/// 通用控制器
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BaseApiController<TEntity,TKey> : ControllerBase where TEntity : Entity<TKey>, IAggregateRoot
{
    /// <summary>
    /// 通用泛型服务
    /// </summary>
    protected readonly IBaseService<TEntity,TKey> Service;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="service">应用层服务</param>
    public BaseApiController(IBaseService<TEntity, TKey> service)
    {
        Service = service;
    }

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <remarks>支持动态排序，动态筛选</remarks>
    /// <param name="searchParams">通用查询参数</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResult<TEntity>> Get([FromQuery] SearchParams searchParams)
    {
        var list = await Service.GetPagedResult(searchParams);
        return list;
    }

    /// <summary>
    /// 获取全部数据
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAll")]
    public async Task<List<TEntity>> GetAll()
    {
        return await Service.GetAll();
    }

    /// <summary>
    /// 根据主键获取
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("GetModelById")]
    public async Task<TEntity?> GetModelById(TKey id)
    {
        return await Service.GetModelById(id);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="model">模型参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(TEntity model)
    {
        var user = await Service.AddEntity(model);
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
        return Ok(await Service.EditEntity(model));
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(TKey id)
    {
        return Ok(await Service.DeleteEntity(id));
    }
}