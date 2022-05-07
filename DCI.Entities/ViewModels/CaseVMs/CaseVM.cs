using DCI.Core;
using DCI.Core.Enums;
using DCI.Entities.Entities;
using DCI.Entities.ViewModels.UserVMs;
using System;
using System.Collections.Generic;

namespace DCI.Entities.ViewModels.CaseVMs
{
    public class CaseVM
    {
        public string Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string CSOUserId { get; set; }
        public List<ViolenceType> ViolenceType { get; set; } 
        public string State { get; set; }
        public string LGA { get; set; }
        public string Statement { get; set; }
        public bool IsFatal { get; set; }
        public bool IsPerpetratorArrested { get; set; }
        public StateOfCase StateOfCase { get; set; }
        public UserVM CSOUser { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public Approval ApprovalAction { get; set; }
        public DateTime LastDateModified { get; set; }
        public string LastModifiedUserId { get; set; }

        public static implicit operator CaseVM(Case model)
        {
            return model == null
                ? null
                : new CaseVM()
                {
                    Id = model.Id,
                    State = model.State,
                    ApprovalAction = model.ApprovalAction,
                    ApprovalStatus = model.ApprovalStatus,
                    CreatedOnUtc = model.CreatedOnUtc,
                    CSOUser = model.CSOUser,
                    CSOUserId = model.CSOUserId,
                    IsFatal = model.IsFatal,
                    IsPerpetratorArrested = model.IsPerpetratorArrested,
                    ViolenceType = model.ViolenceType,
                    StateOfCase = model.StateOfCase,
                    LGA = model.LGA,
                    Statement = model.Statement,
                    LastDateModified = model.LastDateModified,
                    LastModifiedUserId = model.LastModifiedUserId,

                };
        }
    }

}
