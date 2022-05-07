using DCI.Core;
using DCI.Core.Enums;
using DCI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.ViewModels.CaseVMs
{
    public class CaseVMAll
    {
        public string Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public List<ViolenceType> ViolenceType { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Statement { get; set; }
        public bool IsFatal { get; set; }
        public bool IsPerpetratorArrested { get; set; }
        public StateOfCase StateOfCase { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime LastDateModified { get; set; }
        public string LastModifiedUserId { get; set; }

        public static implicit operator CaseVMAll(Case model)
        {
            return model == null
                ? null
                : new CaseVMAll()
                {
                    Id = model.Id,
                    State = model.State,
                    ApprovalStatus = model.ApprovalStatus,
                    CreatedOnUtc = model.CreatedOnUtc,
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
