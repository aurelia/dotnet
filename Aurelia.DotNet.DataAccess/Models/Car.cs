using System;

namespace Aurelia.DotNet.DataAccess.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
