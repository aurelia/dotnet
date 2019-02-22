using Aurelia.DotNet.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Aurelia.DotNet.DataAccess.Models
{
    public partial class User : IdentityUser<int>, IAudit
    {
        public User()
        {
            this.CreateDate = new DateTime();
            this.CreateDateUTC = this.CreateDate.ToUniversalTime();                    
        }

        public virtual string DisplayName => $"{FirstName}{(string.IsNullOrWhiteSpace(MiddleInitial) ? " " : $" {MiddleInitial} ")}{LastName}";

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime LastModifyDateUTC { get; set; }
        public bool IsEnabled { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public virtual ICollection<IdentityUserRole<int>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }

    }
}
