using DCI.Core;
using DCI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.Entities
{
    public class Case
    {
        public Case()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOnUtc = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string CSOUserId { get; set; }
        public List<ViolenceType> ViolenceType { get; set; }=new List<ViolenceType>();
        public string State { get; set; }
        public string LGA { get; set; }
        public string Statement { get; set; }
        public bool IsFatal { get; set; }
        public bool IsPerpetratorArrested { get; set; }
        public StateOfCase StateOfCase { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DCIUser CSOUser { get; set; }
        public string DeletedUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public Approval ApprovalAction { get; set; }
        public string ApprovalActionId { get; set; }
        public DateTime LastDateModified { get; set; }
        public string LastModifiedUserId { get; set; }
    }
}
