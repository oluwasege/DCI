using DCI.Core.ViewModels.MailVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DCI.Services.Interfaces
{
    /// <summary>
    /// Interface IEmailService
    /// </summary>
    public interface IEmailService
    {
        Task<bool> SendMail(List<string> destination, string subject, string body);


        Task<bool> SendEmail(MailRequest mailRequest);
    }
}
