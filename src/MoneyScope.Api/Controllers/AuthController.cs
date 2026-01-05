using Microsoft.AspNetCore.Mvc;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Token;
using MoneyScope.Application.Models.User;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        
        /// <summary>
        /// 
        /// </summary>
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///  Faz o Login do usuário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) =>
             Result(await _userService.AuthenticateUser(model));


        ///// <summary>
        /////  Faz o Login do usuário.
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model) =>
        //     Result(await _userService.AuthenticateUser(model));
    }
}
