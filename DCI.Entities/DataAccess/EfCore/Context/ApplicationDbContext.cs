// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-22-2021
// ***********************************************************************
// <copyright file="ApplicationDbContext.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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

namespace FSDH.Core.DataAccess.EfCore.Context
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
                CreationTime = DateTime.Now,
                PasswordHash = hasher.HashPassword(null, "FSdhAdmin@2021")
            });

            builder.Entity<DCIUser>().HasData(new DCIUser
            {
                Id = ReviewerDCIUserId,
                DCIUserName = "Reviewer@fsdh.com",
                FirstName = "Reviewer",
                LastName = "REVIEWER",
                FullName = "Reviewer@fsdh.com",
                NormalizedDCIUserName = "Reviewer@fsdh.com",
                Email = "Reviewer@fsdh.com",
                NormalizedEmail = "Reviewer@fsdh.com",
                EmailConfirmed = true,
                CreationTime = DateTime.Now,
                PasswordHash = hasher.HashPassword(null, "FSdhReviewer@2021")
            });
            
            builder.Entity<DCIUser>().HasData(new DCIUser
            {
                Id = InforproviderDCIUserId,
                DCIUserName = "Provider@fsdh.com",
                FirstName = "Provider",
                LastName = "Provider",
                FullName = "Provider@fsdh.com",
                NormalizedDCIUserName = "Provider@fsdh.com",
                Email = "Provider@fsdh.com",
                NormalizedEmail = "Provider@fsdh.com",
                EmailConfirmed = true,
                CreationTime = DateTime.Now,
                PasswordHash = hasher.HashPassword(null, "FSdhProvider@2021")
            });

            builder.Entity<DCIUserRole>().HasData(
            new DCIUserRole
            {
                RoleId = AdminRoleId,
                DCIUserId = AdminDCIUserId,
            });

            builder.Entity<DCIUserRole>().HasData(
            new DCIUserRole
            {
                RoleId = ReviewerRoleId,
                DCIUserId = ReviewerDCIUserId,
            });
            
            builder.Entity<DCIUserRole>().HasData(
            new DCIUserRole
            {
                RoleId = InforproviderRoleId,
                DCIUserId = InforproviderDCIUserId,
            });
        }
    }

    /// <summary>
    /// Migration only
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AppDbContextMigrationFactory1 : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// The configuration builder
        /// </summary>
        public static readonly IConfigurationRoot ConfigBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.Development.json", false).Build();

        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>ApplicationDbContext.</returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConfigBuilder.GetConnectionString("Default"),
                
                 b => b.MigrationsAssembly(GetType().Assembly.FullName).EnableRetryOnFailure()
                   
                    )
                .Options);
        }
    }
}