using DCI.Core;
using DCI.Core.Enums;
using DCI.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DCI.Entities.DataAccess
{
    public class ApplicationDbContext: IdentityDbContext<DCIUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Case> Cases { get; set; }
        public DbSet<Approval> Approvals { get; set; }
       // public DbSet<Viole> MyProperty { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedDefaultData(builder);
            base.OnModelCreating(builder);
        }

        private void SeedDefaultData(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<DCIUser>();

            var AdminRoleId = new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210").ToString();
            var AdminDCIUserId = new Guid("b8633e2d-a33b-45e6-8329-1958b3252bbd").ToString();

            var SupervisorRoleId = new Guid("FE8D7501-6CB1-4D99-4C9F-08D9B6BE7D9E").ToString();
            var SupervisorDCIUserId = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();

            var CSORoleId = new Guid("E82FE09A-2419-4B9B-8A2C-B5001E71C997").ToString();
            var CSODCIUserId = new Guid("CDAE8CC2-ADEC-4AB1-4CA0-08D9B6BE7D9E").ToString();
            var CSODCIUserIdSecond = new Guid("EEAE8CC2-ADEC-4AB1-4CA0-08D9B6BE7D9E").ToString();

            var adminRole = AppRoles.AdminRole;
            var cSORole = AppRoles.CSORole;
            var supervisorRole = AppRoles.SupervisorRole;

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = AdminRoleId,
                Name = adminRole,
                NormalizedName = adminRole.ToUpper()
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = SupervisorRoleId,
                Name = supervisorRole,
                NormalizedName = supervisorRole.ToUpper()
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
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
                NormalizedEmail = "Admin@dci.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "DciAdmin@2022"),
                UserType = UserTypes.Admin,
                IsAdmin = true,
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
                PasswordHash = hasher.HashPassword(null, "DciCso@2022"),
                UserType=UserTypes.CSO,
                IsCSO = true,
                State="Lagos"
            });

            builder.Entity<DCIUser>().HasData(new DCIUser
            {
                Id = CSODCIUserIdSecond,
                UserName = "CSOSecond@dci.com",
                FirstName = "Second",
                LastName = "Second",
                NormalizedUserName = "CSOSecond@dci.com",
                Email = "CSOSecond@dci.com",
                NormalizedEmail = "CSOSecond@dci.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "DciCso@2022"),
                UserType = UserTypes.CSO,
                IsCSO = true,
                State="Ogun"
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
                PasswordHash = hasher.HashPassword(null, "DciSupervisor@2022"),
                UserType = UserTypes.Supervisor,
                IsSupervisor = true,
            });
            
            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = AdminRoleId,
                UserId = AdminDCIUserId,
            });

            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = SupervisorRoleId,
                UserId = SupervisorDCIUserId,
            });

            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = CSORoleId,
                UserId = CSODCIUserId,
            });
            builder.Entity<IdentityUserRole<string>>().HasData(
           new IdentityUserRole<string>
           {
               RoleId = CSORoleId,
               UserId = CSODCIUserIdSecond,
           });

            builder.Entity<Case>().HasData(
                new Case
                {
                    Id = new Guid().ToString(),
                    CSOUserId = CSODCIUserId,
                    IsFatal = true,
                    StateOfCase = StateOfCase.Open,
                    LGA = "Lagos",
                    State = "Lagos",
                    Statement = "First Case",
                    IsPerpetratorArrested = false,
                    ApprovalStatus=ApprovalStatus.PENDING
                });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserIdSecond,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Abeokuta",
                   State = "Ogun",
                   Statement = "statement",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserIdSecond,
                   IsFatal = false,
                   StateOfCase = StateOfCase.Closed,
                   LGA = "Ogun",
                   State = "Ogun",
                   Statement = "Second Statement",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Closed,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "Second case",
                   IsPerpetratorArrested = true,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "Third case",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "fourth",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "fifth",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "sixth",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "seventh",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = new Guid().ToString(),
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "eight",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING
               });
        }
    }
}
