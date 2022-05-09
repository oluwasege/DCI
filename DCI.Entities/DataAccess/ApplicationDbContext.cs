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
        //public DbSet<Approval> Approvals { get; set; }
        //public DbSet<ViolenceType> ViolenceTypes { get; set; }
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

            var case1 = new Guid("5E319B4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            var case2 = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7C9E").ToString();
            var case3 = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7D9A").ToString();
            var case4 = new Guid("5E21DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            var case5 = new Guid("4E31DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            var case6 = new Guid("5E31DB4E-6A79-487E-4C9E-08D9B6BE7D9E").ToString();
            var case7 = new Guid("5E31DB4E-6E79-487E-4C9A-08D9B6BE7D9E").ToString();
            var case8 = new Guid("5E31DB4E-6E79-487E-5C9E-08D9B6BE7D9E").ToString();
            var case9 = new Guid("5E31DB4E-6E79-487E-4C9E-18D9B6BE7D9E").ToString();
            var case10 = new Guid("5E31DB4E-6E79-437E-4C9E-08D9B6BE7D9E").ToString();

            var approve1 = new Guid("5E319B4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            var approve2 = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7C9E").ToString();
            var approve3 = new Guid("5E31DB4E-6E79-487E-4C9E-08D9B6BE7D9A").ToString();
            var approve4 = new Guid("5E21DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            var approve5 = new Guid("4E31DB4E-6E79-487E-4C9E-08D9B6BE7D9E").ToString();
            var approve6 = new Guid("5E31DB4E-6A79-487E-4C9E-08D9B6BE7D9E").ToString();
            var approve7 = new Guid("5E31DB4E-6E79-487E-4C9A-08D9B6BE7D9E").ToString();
            var approve8 = new Guid("5E31DB4E-6E79-487E-5C9E-08D9B6BE7D9E").ToString();
            var approve9 = new Guid("5E31DB4E-6E79-487E-4C9E-18D9B6BE7D9E").ToString();
            var approve10 = new Guid("5E31DB4E-6E79-437E-4C9E-08D9B6BE7D9E").ToString();

            var CSORoleId = new Guid("E82FE09A-2419-4B9B-8A2C-B5001E71C997").ToString();
            var CSODCIUserId = new Guid("CDAE8CC2-ADEC-4AB1-4CA0-08D9B6BE7D9E").ToString();
            var CSODCIUserIdSecond = new Guid("EEAE8CC2-ADEC-4AB1-4CA0-08D9B6BE7D9E").ToString();

            var adminRole = AppRoles.AdminRole;
            var cSORole = AppRoles.CSORole;
            var supervisorRole = AppRoles.SupervisorRole;

            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve1,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve2,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve3,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve4,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve5,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve6,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve7,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve8,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve9,
                RejectedBy = null,

            });
            builder.Entity<Approval>().HasData(new Approval
            {
                ActionComment = null,
                Id = approve10,
                RejectedBy = null,

            });

            var violenceId = new Guid().ToString();
            builder.Entity<ViolenceType>().HasData(new ViolenceType
            {
                Id = violenceId,
                ChildAbuse = true,
                CyberBullying = true,
                Defilement = true,
                DenialOfResources = true,
                EarlyMarriage = true,
                FemaleGenitalMutilation = true,
                ForcedMarriage = true,
                PhysicalAssault = true,
                Psychological = true,
                Rape = true,
                SocialAssault = true,
                ViolationOfProperty = true
            });

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
                State = "Lagos"
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
                State = "Lagos"
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
                UserId = AdminDCIUserId,
            });
                        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = CSORoleId,
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
                    Id = case1,
                    CSOUserId = CSODCIUserId,
                    IsFatal = true,
                    StateOfCase = StateOfCase.Open,
                    LGA = "Lagos",
                    State = "Lagos",
                    Statement = "First Case",
                    IsPerpetratorArrested = false,
                    ApprovalStatus=ApprovalStatus.PENDING,
                    ViolenceTypeId= violenceId,
                    ApprovalActionId=approve1
                });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case2,
                   CSOUserId = CSODCIUserIdSecond,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Abeokuta",
                   State = "Ogun",
                   Statement = "statement",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve2
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case3,
                   CSOUserId = CSODCIUserIdSecond,
                   IsFatal = false,
                   StateOfCase = StateOfCase.Closed,
                   LGA = "Ogun",
                   State = "Ogun",
                   Statement = "Second Statement",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve3
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case4,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Closed,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "Second case",
                   IsPerpetratorArrested = true,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve4
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case5,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "Third case",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve5
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id =case6,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "fourth",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve6
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case7,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "fifth",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve7
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case8,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "sixth",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve8
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case9,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "seventh",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve9
               });
            builder.Entity<Case>().HasData(
               new Case
               {
                   Id = case10,
                   CSOUserId = CSODCIUserId,
                   IsFatal = true,
                   StateOfCase = StateOfCase.Open,
                   LGA = "Lagos",
                   State = "Lagos",
                   Statement = "eight",
                   IsPerpetratorArrested = false,
                   ApprovalStatus = ApprovalStatus.PENDING,
                   ViolenceTypeId = violenceId,
                   ApprovalActionId = approve10
               });
        }
    }
}
