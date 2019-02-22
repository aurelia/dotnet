using System.Collections.Generic;

namespace Aurelia.DotNet.DataAccess.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}