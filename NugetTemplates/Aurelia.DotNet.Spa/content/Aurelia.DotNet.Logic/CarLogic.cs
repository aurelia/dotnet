using Aurelia.DotNet.DataAccess;
using Aurelia.DotNet.Logic.Interfaces;
using Aurelia.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Logic
{
    public class VehicleLogic : IVehicleLogic
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleLogic> _logger;
        private readonly IPrincipal _user;

        public VehicleLogic(ApplicationDbContext context, ILogger<VehicleLogic> logger, IPrincipal user)
        {
            _context = context;
            _logger = logger;
            _user = user;
        }

        public async Task<IEnumerable<Vehicle>> GetAllCarsAsync()
        {            
            _logger.LogInformation("Getting All Cars");
            // We could use AutoMapper if we wanted to
            return (await _context.Vehicles.Include(y => y.Manufacturer).ToListAsync().ConfigureAwait(false)).Select(vehicle => new Vehicle
            {
                Id = vehicle.Id,
                Trim = vehicle.Trim,
                ManufacturerId = vehicle.ManufacturerId,
                Model = vehicle.Model,
                Price = vehicle.Price,
                Year = vehicle.Year,
                Manufacturer = vehicle.Manufacturer == null ? null : new Manufacturer
                {
                    Id = vehicle.Manufacturer.Id,
                    Name = vehicle.Manufacturer.Name
                }
            });

        }
    }
}
