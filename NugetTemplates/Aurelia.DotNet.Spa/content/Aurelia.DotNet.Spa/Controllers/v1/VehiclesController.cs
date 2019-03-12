#if (Secure)

using Aurelia.DotNet.Logic;
using Aurelia.DotNet.Logic.Interfaces;
using Aurelia.DotNet.Models;
using Aurelia.DotNet.Spa.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Spa.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class VehiclesController : Controller
    {
        private readonly IVehicleLogic _logic;

        public VehiclesController(IVehicleLogic logic) => _logic = logic;

        [HttpGet]
        [Authorize(Policies.GetVehicles)]
        [Produces(typeof(Vehicle))]
        public async Task<IActionResult> GetVehicles() => Ok(await _logic.GetAllCarsAsync());
    }
}
#endif