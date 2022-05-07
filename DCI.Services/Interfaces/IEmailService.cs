using DCI.Entities.ViewModels.MailVMs;
using System.Collections.Generic;
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
