using Microsoft.AspNetCore.Mvc;
using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.User;

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
        [HttpPost("CreateUser")]
        public async Task<IActionResult> Add([FromBody] CreateUserModel model) =>
             Result(await _userService.Add(model));

        /// <summary>
        /// Atualiza um usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel model) =>
             Result(await _userService.Update(model));


        /// <summary>
        /// Atualiza o status de um usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeUserStatusModel model) =>
             Result(await _userService.ChangeStatus(model));


        /// <summary>
        /// Retorna um usuario por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromBody] long id) =>
             Result(await _userService.GetById(id));


        /// <summary>
        /// Retorna um usuario por id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetPaginated([FromBody] UserFilterModel model) =>
             Result(await _userService.GetPaginated(model));

    }
}
