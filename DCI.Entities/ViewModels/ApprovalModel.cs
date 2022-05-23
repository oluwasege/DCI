using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DCI.Entities.ViewModels
{
    public class ApprovalModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(500, ErrorMessage = "Comment should be a maximum of 250 characters")]
        public string Comment { get; set; }
    }
}
