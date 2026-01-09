using Microsoft.AspNetCore.Mvc;
using MoneyScope.Api.Authorization;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;
using System.Globalization;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class SendEmailController : BaseController
    {
        private readonly ISendEmailService _sendEmailService;

        /// <summary>
        /// 
        /// </summary>
        public SendEmailController(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        /// <summary>
        /// Envia um email de recuperação de senha.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "SendEmail-C" })]
        [HttpPost]
        public async Task<IActionResult> SendEmailRecoveryPassword ([FromQuery] String email) =>
             Result(await _sendEmailService.SendEmailResetPassword(email));


        /// <summary>
        /// Altera a senha do usuario.
        /// </summary>
        /// <param name="model"> Token enviado por email e nova senha </param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "SendEmail-E" })]
        [HttpPatch]
        public async Task<IActionResult> UpdatePassword([FromQuery] ResetPasswordModel model) =>
            Result(await _sendEmailService.ResetPassword(model.Token, model.NewPassword));
    }
}
