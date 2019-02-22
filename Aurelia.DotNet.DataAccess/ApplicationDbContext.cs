using Aurelia.DotNet.DataAccess.Interfaces;
using Aurelia.DotNet.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aurelia.DotNet.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public int UserId { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Manufacturer> Manufacturers{ get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Role>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Role>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Vehicle>().HasOne(y => y.Manufacturer).WithMany(y => y.Vehicles).HasForeignKey(y=>y.ManufacturerId).HasConstraintName("FK_Vehicle_ManufacturerId_Manufacturer_Id");
        }

        public override int SaveChanges()
        {
            AuditEntitiesBeingSaved();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditEntitiesBeingSaved();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AuditEntitiesBeingSaved();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AuditEntitiesBeingSaved();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void AuditEntitiesBeingSaved()
        {
            var currentTime = DateTime.Now;
            ChangeTracker.Entries().Where(x => x.Entity is IAudit && (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList().ForEach(changeEntry =>
            {
                var auditEntity = changeEntry.Entity as IAudit;
                if (changeEntry.State == EntityState.Added)
                {
                    auditEntity.CreateDate = currentTime;
                    auditEntity.CreateDateUTC = currentTime.ToUniversalTime();
                    auditEntity.CreatedBy = UserId;
                }
                else
                {
                    // Whoa Nelly - You can't set these again!
                    base.Entry(auditEntity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(auditEntity).Property(x => x.CreateDate).IsModified = false;
                    base.Entry(auditEntity).Property(x => x.CreateDateUTC).IsModified = false;
                }

                auditEntity.LastModifyDate = currentTime;
                auditEntity.LastModifyDate = currentTime.ToUniversalTime();
                auditEntity.LastModifiedBy = UserId;
            });
        }

    }
}
