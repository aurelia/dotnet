using Aurelia.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Logic.Interfaces
{
    public interface IVehicleLogic
    {
        Task<IEnumerable<Vehicle>> GetAllCarsAsync();
    }
}
