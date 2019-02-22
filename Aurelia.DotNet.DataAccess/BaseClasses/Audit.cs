using Aurelia.DotNet.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurelia.DotNet.DataAccess
{
    public abstract class Audit : IAudit
    {

        public Audit()
        {
            this.CreateDate = new DateTime();
            this.CreateDateUTC = this.CreateDate.ToUniversalTime();
        }


        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime LastModifyDateUTC { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
