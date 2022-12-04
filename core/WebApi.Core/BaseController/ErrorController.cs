using Application.Core.DTO;
using Application.Core.MyException;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Core.BaseController;

/// <summary>
/// 全局异常处理
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
[Route("[controller]")]
[ApiController]
public class ErrorController : ControllerBase
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public IActionResult Get()
    {
        IExceptionHandlerPathFeature? iExceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (iExceptionHandlerFeature != null)
        {
            var exception = iExceptionHandlerFeature.Error;
            if (exception is MyException)
            {
                return BadRequest(new ExceptionResult
                {
                    Message = exception.Message
                });
            }
            else if(exception is NotFoundException)
            {
                return NotFound(new ExceptionResult
                {
                    Message = exception.Message
                });
            }
                
        }
        return StatusCode(StatusCodes.Status500InternalServerError,new ExceptionResult
        {
            Message="未经处理的异常！"
        });
    }
}