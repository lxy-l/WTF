using Crafty.Application.Core.ApplicationServices;
using Crafty.WebApi.Core.BaseController;

using Domain.Entities;

namespace WebApi.Controllers
{
    /// <summary>
    /// 年休假管理
    /// </summary>
    public class AnnualLeaveController : BaseApiController<AnnualLeave, int>
    {
        public AnnualLeaveController(IBaseService<AnnualLeave, int> service) : base(service)
        {
        }
    }
}
