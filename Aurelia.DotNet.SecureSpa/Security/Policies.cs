using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Aurelia.DotNet.Spa.Security
{
    public static class Policies
    {
        public const string GetVehicles = "View Vehicles";
        public const string Default = "default"; // Required to access the application at all
    }
}
