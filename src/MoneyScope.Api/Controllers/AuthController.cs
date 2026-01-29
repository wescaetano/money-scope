using Microsoft.AspNetCore.Mvc;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Auth;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Application.Models.Token;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// 
        /// </summary>
        public AuthController(IAuthService userService, ITokenService tokenService)
        {
            _authService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        ///  Faz o Login do usuário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) =>
             Result(await _authService.AuthenticateUser(model));

        /// <summary>
        ///  Faz o Login social do usuário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SocialLogin")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginModel model) =>
             Result(await _authService.SocialLogin(model));


        /// <summary>
        /// Envia um email de recuperação de senha.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //[APIAuthorization(new string[] { "SendEmail-C" })]
        [HttpPost]
        public async Task<IActionResult> SendEmailRecoveryPassword([FromQuery] String email) =>
             Result(await _authService.SendEmailResetPassword(email));


        /// <summary>
        /// Altera a senha do usuario.
        /// </summary>
        /// <param name="model"> Token enviado por email e nova senha </param>
        /// <returns></returns>
        //[APIAuthorization(new string[] { "SendEmail-E" })]
        [HttpPatch]
        public async Task<IActionResult> UpdatePassword([FromQuery] ResetPasswordModel model) =>
            Result(await _authService.ResetPassword(model.Token, model.NewPassword));

        /// <summary>
        /// Refresh access token and refreshToken.
        /// </summary>
        /// <param name="request"> </param>
        /// <returns></returns>
        [HttpPatch("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestModel? request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Token))
                return BadRequest("Token inválido!");

            var response = await _tokenService.ValidateRefreshToken(request.Token);
            if (response == null)
                return BadRequest("Não foi possível validar o refresh token.");

            return Result(response);
        }

        ///// <summary>
        ///// exclude expired refresh tokens.
        ///// </summary>
        ///// <returns></returns>
        //[HttpDelete("Exclude")]
        //public async Task<IActionResult> ExcludeRefreshTokens() =>
        //    Result(await _tokenService.ExcludeExpiredRefreshTokens());


    }
}
