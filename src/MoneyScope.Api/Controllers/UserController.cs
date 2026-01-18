using Microsoft.AspNetCore.Mvc;
using MoneyScope.Api.Authorization;
using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Adiciona um usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[APIAuthorization(new string[] { "Users-C" })]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> Add([FromBody] CreateUserModel model) =>
             Result(await _userService.Add(model));

        /// <summary>
        /// Atualiza um usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Users-E" })]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel model) =>
             Result(await _userService.Update(model));


        /// <summary>
        /// Atualiza o status de um usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Users-I" })]
        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] ChangeUserStatusModel model) =>
             Result(await _userService.ChangeStatus(model));


        /// <summary>
        /// Retorna um usuario por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Users-V" })]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] long id) =>
             Result(await _userService.GetById(id));


        /// <summary>
        /// Retorna um usuario por id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Users-V" })]
        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] UserFilterModel model) =>
             Result(await _userService.GetPaginated(model));

    }
}
