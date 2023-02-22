using Application.Core.ApplicationServices;

using Domain.Entities;

using WebApi.Core.BaseController;

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
