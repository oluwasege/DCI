using DCI.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DCI.Entities
{
    [Table(nameof(DCIUser))]
    public class DCIUser: IdentityUser<Guid>
    {
        public DCIUser()
        {
            Id = Guid.NewGuid();
            CreatedOnUtc = DateTime.UtcNow;
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        //public string AccountName { get; set; }
        public string State { get; set; }
        public Gender Gender { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Activated { get; set; }
        public bool IsDeleted { get; set; }
        public UserTypes UserType { get; set; }
        public Guid? UserTypeId { get; set; }
        public bool IsFirstTimeLogin { get; set; }
    }

    public class DCIUserClaim : IdentityUserClaim<Guid>
    {
    }

    public class DCIUserLogin : IdentityUserLogin<Guid>
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }

    public class DCIRole : IdentityRole<Guid>
    {
        public DCIRole()
        {
            Id = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        public UserTypes UserType { get; set; }
        public bool IsCSO { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSupervisor { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }

    public class DCIUserRole : IdentityUserRole<Guid>
    {

    }

    public class DCIRoleClaim : IdentityRoleClaim<Guid>
    {
    }

    public class DCIUserToken : IdentityUserToken<Guid>
    {
    }
}
