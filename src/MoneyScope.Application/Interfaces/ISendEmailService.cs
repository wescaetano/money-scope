using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Core.Enums.SendEmail;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface ISendEmailService
    {
        Task<ResponseModel<dynamic>> SendEmailResetPassword(string email, ERedefinitionEmailType type = ERedefinitionEmailType.RequestToResetPassword);
        Task<bool> SendEmail(SendEmailModel model, ERedefinitionEmailType type, string name);
        Task<bool> SendGenericEmail(string to, string subject, string titulo, string texto1, string texto2);
        Task<ResponseModel<dynamic>> ResetPassword(string token, string newPassword);
    }
}
