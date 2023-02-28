using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// 异步编程示例
/// </summary>
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class TaskController : ControllerBase
{
    private ILogger<TaskController> Logger { get; }

    public TaskController(ILogger<TaskController> logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// 异步编程示例
    /// </summary>
    /// <remarks>添加CancellationToken参数后，如果客户端取消请求，则可以立即停止Task</remarks>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken=default)
    {
        await Task.Delay(5000, cancellationToken)
            .ContinueWith(x =>
            {
                //这里的Task不会阻塞主线程
                Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("任务开始");
                    await Task.Delay(5000, cancellationToken);
                    Console.WriteLine("任务结束");
                }, cancellationToken);

                if (x.IsCanceled)
                {
                    Logger.LogInformation("取消了");
                }
                else if(x.IsCompletedSuccessfully)
                {
                    Logger.LogInformation("5秒后：执行了");
                }

            }, cancellationToken);
        return Ok("OK");
    }
}