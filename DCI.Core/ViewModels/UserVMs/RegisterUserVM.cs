using DCI.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DCI.Core.ViewModels.UserVMs
{
    public class RegisterUserVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]  
        public string PhoneNumber { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
