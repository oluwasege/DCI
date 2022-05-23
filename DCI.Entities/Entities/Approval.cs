using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.Entities
{
    public class Approval
    {
        public Approval()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOnUtc = DateTime.UtcNow;
        }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime LastDateModified { get; set; }
        public string  Id { get; set; }
        public string ApprovedBy { get; set; }
        public string RejectedBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionComment { get; set; }     
    }

   
}
