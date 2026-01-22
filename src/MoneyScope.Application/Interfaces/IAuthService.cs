using MoneyScope.Application.Models.Report;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Application.Models.Token;
using MoneyScope.Core.Enums.SendEmail;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel<dynamic>> AuthenticateUser(LoginModel model);
        Task<ResponseModel<dynamic>> AuthenticateSocialUser(LoginModel model);
        Task<ResponseModel<dynamic>> SendEmailResetPassword(string email, ERedefinitionEmailType type = ERedefinitionEmailType.RequestToResetPassword);
        Task<bool> SendEmail(SendEmailModel model, ERedefinitionEmailType type, string name);
        Task<bool> SendGenericEmail(string to, string subject, string titulo, string texto1, string texto2, List<EmailAttachment>? attachments = null);
        Task<ResponseModel<dynamic>> ResetPassword(string token, string newPassword);
    }
}
