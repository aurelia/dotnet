using Aurelia.DotNet.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurelia.DotNet.DataAccess.Models
{
    public partial class Role : IdentityRole<int>, IAudit
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }


        public Role(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime LastModifyDateUTC { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public virtual ICollection<IdentityUserRole<int>> Users { get; set; }
        public virtual ICollection<IdentityRoleClaim<int>> Claims { get; set; }
    }

}
