using Crafty.Application.Core.ApplicationServices;
using Crafty.WebApi.Core.BaseController;

using Domain.Entities;

using Infrastructure.Context;

namespace WebApi.Controllers
{
    /// <summary>
    /// 年休假管理
    /// </summary>
    public class AnnualLeaveController : BaseApiController<UserDbContext, AnnualLeave, int>
    {
        public AnnualLeaveController(IBaseService<UserDbContext, AnnualLeave, int> baseService) : base(baseService)
        {
        }
    }
}
