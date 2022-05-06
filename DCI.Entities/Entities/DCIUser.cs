using DCI.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DCI.Entities.Entities
{
    public class DCIUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string AvatarUrl { get; set; }
        public DCIUser()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOnUtc = DateTime.UtcNow;
        }
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
        public bool IsAdmin { get; set; }
        public bool IsCSO { get; set; }
        public bool IsSupervisor { get; set; }
    }
}
