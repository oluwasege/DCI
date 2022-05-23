using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Core
{
    public class ViolenceType
    {
        public ViolenceType()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public bool PhysicalAssault { get; set; }
        public bool Defilement { get; set; }
        public bool Rape { get; set; }
        public bool ForcedMarriage { get; set; }
        public bool DenialOfResources { get; set; }
        public bool Psychological { get; set; }
        public bool SocialAssault { get; set; }
        public bool FemaleGenitalMutilation { get; set; }
        public bool ViolationOfProperty { get; set; }
        public bool ChildAbuse { get; set; }
        public bool EarlyMarriage { get; set; }
        public bool CyberBullying { get; set; }
    }
}
