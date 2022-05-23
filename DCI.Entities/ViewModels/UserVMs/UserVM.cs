using DCI.Core.Enums;
using DCI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.ViewModels.UserVMs
{
    public class UserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        //public string AccountName { get; set; }
        public string State { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserTypes UserType { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCSO { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsActive { get; set; }
        public IList<string> Roles { get; set; }

        public static implicit operator UserVM(DCIUser model)
        {
            return model == null
                ? null
                : new UserVM()
                {
                    Id = model.Id,
                    DateOfBirth = model.DateOfBirth,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    IsAdmin = model.IsAdmin,
                    IsCSO = model.IsCSO,
                    IsSupervisor = model.IsSupervisor,
                    LastName = model.LastName,
                    State = model.State,
                    UserType = model.UserType,
                    IsActive=model.Activated
                };
        }
    }
}
