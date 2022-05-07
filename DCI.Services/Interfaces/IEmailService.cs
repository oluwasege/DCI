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
        //Task<bool> SendEmailAsync(string email, string subject, string message);
        /// <summary>
        /// Generates the email confirmation link asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="email">The email.</param>
        /// <returns>System.String.</returns>
        string GenerateEmailConfirmationLinkAsync(string token, string email);

        //string GeneratePasswordResetLinkAsync(string token, string email);
        //string ChangeAdminDefaultPasswordUrlLink(string email);
        /// <summary>
        /// Sends the account verification email.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="confirmationLink">The confirmation link.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> SendAccountVerificationEmail(string userEmail, string firstName, string subject, string confirmationLink);
        //Task<bool> ActionNotification(string userEmail, string firstName, string subject, string companyName, string url);
        //Task<bool> SendPasswordResetEmail(string email, string subject, string passwordResetLink);
        //Task<bool> SendAdminPasswordResetEmail(string email, string subject, string passwordResetUrl);
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="replacements">The replacements.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="emailTemplatePath">The email template path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> SendMail(List<string> destination, string[] replacements, string subject, string emailTemplatePath);


        Task<bool> SendEmail(MailRequest mailRequest);
    }
}
