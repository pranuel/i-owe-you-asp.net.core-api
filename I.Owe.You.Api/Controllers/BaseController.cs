using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace I.Owe.You.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected string UserSub => this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
