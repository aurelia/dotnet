using System;
using System.Collections.Generic;
using System.Text;

namespace Aurelia.DotNet.DataAccess.Interfaces
{
    public interface IAudit
    {
        DateTime CreateDate { get; set; }
        DateTime CreateDateUTC { get; set; }
        DateTime LastModifyDate { get; set; }
        DateTime LastModifyDateUTC { get; set; }
        int CreatedBy { get; set; }
        int LastModifiedBy { get; set; }
    }
}
