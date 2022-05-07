using System.Collections.Generic;

namespace DCI.Core.Utils
{
    /// <summary>
    /// Class CoreConstants.
    /// </summary>
    public abstract class CoreConstants
    {
        /// <summary>
        /// The date format
        /// </summary>
        public const string DateFormat = "dd MMMM, yyyy";
        /// <summary>
        /// The time format
        /// </summary>
        public const string TimeFormat = "hh:mm tt";
        /// <summary>
        /// The system date format
        /// </summary>
        public const string SystemDateFormat = "dd/MM/yyyy";
        /// <summary>
        /// The permission
        /// </summary>
        public const string Permission = nameof(Permission);
        /// <summary>
        /// The role
        /// </summary>
        public const string Role = "role";

        /// <summary>
        /// The UserIdKey
        /// oid is used by Azure AD
        /// </summary>
        public const string UserIdKey = "oid";

        /// <summary>
        /// The valid excels
        /// </summary>
        public static readonly string[] validExcels = { ".xls", ".xlsx" };

        /// <summary>
        /// PDF Templates
        /// </summary>
        public const string TestPdfTemplatePath1 = @"filestore\pdftemplate\TestPdfTemplate1.html";
        public const string TestPdfTemplatePath2 = @"filestore\pdftemplate\TestPdfTemplate2.html";
        public const string ConfirmEmail = @"Filestore\EmailTemplate\Confirm_email.html";

        /// <summary>
        /// The email templates
        /// </summary>
        public static readonly List<EmailTemplate> EmailTemplates = new List<EmailTemplate>
        {
            new EmailTemplate(MailUrl.PasswordReset, "Password Reset", "filestore/emailtemplates/passwordreset.htm")
        };

        /// <summary>
        /// Class MailUrl.
        /// </summary>
        public static class MailUrl
        {
            /// <summary>
            /// The password reset
            /// </summary>
            public const string PasswordReset = "filestore/emailtemplates/passwordreset.htm";
        }

        /// <summary>
        /// Class EmailTemplate.
        /// </summary>
        public class EmailTemplate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EmailTemplate"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="subject">The subject.</param>
            /// <param name="template">The template.</param>
            public EmailTemplate(string name, string subject, string template)
            {
                Subject = subject;
                TemplatePath = template;
                Name = name;
            }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }
            /// <summary>
            /// Gets or sets the subject.
            /// </summary>
            /// <value>The subject.</value>
            public string Subject { get; set; }
            /// <summary>
            /// Gets or sets the template path.
            /// </summary>
            /// <value>The template path.</value>
            public string TemplatePath { get; set; }
        }

        /// <summary>
        /// Class PaginationConsts.
        /// </summary>
        public static class PaginationConsts
        {
            /// <summary>
            /// The page size
            /// </summary>
            public const int PageSize = 25;
            /// <summary>
            /// The page index
            /// </summary>
            public const int PageIndex = 1;
        }

        /// <summary>
        /// Class AllowedFileExtensions.
        /// </summary>
        public static class AllowedFileExtensions
        {
            /// <summary>
            /// The signature
            /// </summary>
            public const string Signature = ".jpg,.png";
        }

        /// <summary>
        /// Class CacheConstants.
        /// </summary>
        public static class CacheConstants
        {
            /// <summary>
            /// Class Keys.
            /// </summary>
            public static class Keys
            {
                /// <summary>
                /// The user claims
                /// </summary>
                public const string UserClaims = "userclaims";
                /// <summary>
                /// The role permissions
                /// </summary>
                public const string RolePermissions = "rolepermissions";
                /// <summary>
                /// The role
                /// </summary>
                public const string UserRole = "role";
            }

            /// <summary>
            /// Class CacheTime.
            /// </summary>
            public static class CacheTime
            {
                /// <summary>
                /// The month in minutes
                /// </summary>
                public const int MonthInMinutes = 43800;
            }
        }
    }
}