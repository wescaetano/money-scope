using Microsoft.AspNetCore.Mvc;
using MoneyScope.Api.Authorization;
using MoneyScope.Application.Filters.Goal;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Goal;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class GoalController : BaseController
    {
        private readonly IGoalService _goalService;
        
        /// <summary>
        /// 
        /// </summary>
        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        /// <summary>
        /// Adiciona uma meta.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Goals-C" })]
        [HttpPost("Create")]
        public async Task<IActionResult> Add([FromQuery] CreateGoalModel model) =>
             Result(await _goalService.Add(model));

        /// <summary>
        /// Altera uma meta.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Goals-E" })]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] UpdateGoalModel model) =>
             Result(await _goalService.Update(model));


        /// <summary>
        /// Altera o status de uma meta.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Goals-I" })]
        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] ChangeGoalStatusModel model) =>
             Result(await _goalService.ChangeStatus(model));


        /// <summary>
        /// Retorna uma meta por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Goals-V" })]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] long id) =>
             Result(await _goalService.GetById(id));


        /// <summary>
        /// Retorna uma listagem de metas.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Goals-V" })]
        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] GoalFilterModel model) =>
             Result(await _goalService.GetPaginated(model));
    }
}
