using DCI.Core;
using DCI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.ViewModels.CaseVMs
{
    public class SubmitCaseVM
    {
        public ViolenceTypeVM ViolenceType { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Statement { get; set; }
        public bool IsFatal { get; set; }
        public bool IsPerpetratorArrested { get; set; }
        public StateOfCase StateOfCase { get; set; }
    }

    public class ViolenceTypeVM
    {
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

        public static implicit operator ViolenceTypeVM(ViolenceType model)
        {
            return model == null
                ? null
                : new ViolenceTypeVM()
                {
                    ChildAbuse = model.ChildAbuse,
                    CyberBullying = model.CyberBullying,
                    Defilement = model.Defilement,
                    DenialOfResources = model.DenialOfResources,
                    EarlyMarriage = model.EarlyMarriage,
                    FemaleGenitalMutilation = model.FemaleGenitalMutilation,
                    ForcedMarriage = model.ForcedMarriage,
                    PhysicalAssault = model.PhysicalAssault,
                    Psychological = model.Psychological,
                    Rape = model.Rape,
                    SocialAssault = model.SocialAssault,
                    ViolationOfProperty = model.ViolationOfProperty


                };
        }
    }

    public class ViolenceTypeSubmitVM
    {
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
