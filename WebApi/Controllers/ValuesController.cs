using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ILogger<ValuesController> _logger;

    public ValuesController(ILogger<ValuesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken=default)
    {
        await Task.Delay(5000, cancellationToken)
            .ContinueWith(x =>
            {
                //这里的Task不会阻塞
                Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine("任务开始");
                    await Task.Delay(5000);
                    Console.WriteLine("任务结束");
                });

                if (x.IsCanceled)
                {
                    _logger.LogInformation("取消了");
                }
                else if(x.IsCompletedSuccessfully)
                {
                    _logger.LogInformation("5秒后：执行了");
                }

            });
        return Ok("OK");
    }
}