using System;
using System.Collections.Generic;
using System.Text;

namespace Aurelia.DotNet.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Cars = new HashSet<Vehicle>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Vehicle> Cars { get; set; }
    }
}