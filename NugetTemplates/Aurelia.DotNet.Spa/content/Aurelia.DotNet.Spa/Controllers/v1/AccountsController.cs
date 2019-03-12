#if (Secure)
using Microsoft.AspNetCore.Mvc;

namespace Aurelia.DotNet.Spa.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountsController : Controller
    {
    }
}
#endif