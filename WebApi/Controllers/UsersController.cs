using Application.ApplicationServices;

using Domain.Entities;

using WebApi.BaseController;

namespace WebApi.Controllers
{
    public class UsersController : BaseApiController<User, int>
    {
        public UsersController(IBaseService<User, int> service) : base(service)
        {
        }
    }
}
