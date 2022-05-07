using DCI.Core;
using DCI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.ViewModels.CaseVMs
{
    public class SubmitCaseVM
    {
        public List<ViolenceType> ViolenceType { get; set; } = new List<ViolenceType>();
        public string State { get; set; }
        public string LGA { get; set; }
        public string Statement { get; set; }
        public bool IsFatal { get; set; }
        public bool IsPerpetratorArrested { get; set; }
        public StateOfCase StateOfCase { get; set; }
    }
}
