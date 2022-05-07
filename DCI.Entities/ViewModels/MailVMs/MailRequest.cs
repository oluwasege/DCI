using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.ViewModels.MailVMs
{
    public class MailRequest
    {
        public List<string> Recipient { get; set; }
        public List<string> BCC { get; set; }
        public List<string> CC { get; set; }
        public bool IsHtmlBody { get; set; } = true;
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
