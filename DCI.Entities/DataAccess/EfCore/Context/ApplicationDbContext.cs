using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using FSDH.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FSDH.Entities.Enums;
using DCI.Entities;
using DCI.Core.Enums;

namespace DCI.Entities.DataAccess.EfCore.Context
{
    /// <summary>
    /// Class ApplicationDbContext.
    /// Implements the <see cref="FSDH.Core.DataAccess.EfCore.Context.BaseDbContext" />
    /// </summary>
    /// <seealso cref="FSDH.Core.DataAccess.EfCore.Context.BaseDbContext" />
    /// <Note>
    /// DbSet properties are being used by generic repository
    /// </Note>
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : BaseDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

            public DbSet<DCIDCIUser> SectorArticles { get; set; }

        /// <summary>
        /// Called when [model creating].
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseOpenIddict();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

           SeedDefaultData(builder);
            base.OnModelCreating(builder);
        }

        private void SeedDefaultData(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<DCIUser>();

            var AdminRoleId = new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210");
            var AdminDCIUserId = new Guid("b8633e2d-a33b-45e6-8329-1958b3252bbd");
            
            var SupervisorRoleId = new Guid("FE8D7501-6CB1-4D99-4C9F-08D9B6BE7D9E");
            var SupervisorDCIUserId = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7D9E");
            
            var CSORoleId = new Guid("E82FE09A-2419-4B9B-8A2C-B5001E71C997");
            var CSODCIUserId = new Guid("CDAE8CC2-ADEC-4AB1-4CA0-08D9B6BE7D9E");

            var adminRole = AppRoles.AdminRole;
            var cSORole = AppRoles.CSORole;
            var supervisorRole = AppRoles.SupervisorRole;

            builder.Entity<DCIRole>().HasData(new DCIRole
            {
                Id = AdminRoleId,
                Name = adminRole,
                NormalizedName = adminRole.ToUpper()
            });
            builder.Entity<DCIRole>().HasData(new DCIRole
            {
                Id = SupervisorRoleId,
                Name = supervisorRole,
                NormalizedName = supervisorRole.ToUpper()
            });
            
            builder.Entity<DCIRole>().HasData(new DCIRole
            {
                Id = CSORoleId,
                Name = cSORole,
                NormalizedName = cSORole.ToUpper()
            });
            

            builder.Entity<DCIUser>().HasData(new DCIUser
            {
                Id = AdminDCIUserId,
                UserName = "Admin@dci.com",
                FirstName = "Admin",
                LastName = "Admin",
                NormalizedUserName = "Admin@dci.com",
                Email = "Admin@dci.com",
                NormalizedEmail = "Admin@fsdh.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "DciAdmin@2021")
            });

            builder.Entity<DCIUser>().HasData(new DCIUser
            {
                Id = CSODCIUserId,
                UserName = "CSO@dci.com",
                FirstName = "CSO",
                LastName = "CSO",
                NormalizedUserName = "CSO@dci.com",
                Email = "CSO@dci.com",
                NormalizedEmail = "CSO@dci.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "DciCso@2021")
            });
            
            builder.Entity<DCIUser>().HasData(new DCIUser
            {
                Id = SupervisorDCIUserId,
                UserName = "Supervisor@dci.com",
                FirstName = "Supervisor",
                LastName = "Supervisor",
                NormalizedUserName = "Supervisor@dci.com",
                Email = "Supervisor@dci.com",
                NormalizedEmail = "Supervisor@dci.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "DciSupervisor@2021")
            });

            builder.Entity<DCIUserRole>().HasData(
            new DCIUserRole
            {
                RoleId = AdminRoleId,
                UserId = AdminDCIUserId,
            });

            builder.Entity<DCIUserRole>().HasData(
            new DCIUserRole
            {
                RoleId = SupervisorRoleId,
                UserId = SupervisorDCIUserId,
            });
            
            builder.Entity<DCIUserRole>().HasData(
            new DCIUserRole
            {
                RoleId = CSORoleId,
                UserId = CSODCIUserId,
            });
        }
    }
}