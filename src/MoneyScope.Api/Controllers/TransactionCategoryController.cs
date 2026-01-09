using Microsoft.AspNetCore.Mvc;
using MoneyScope.Api.Authorization;
using MoneyScope.Application.Filters.TransactionCategory;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.TransactionCategory;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class TransactionCategoryController : BaseController
    {
        private readonly ITransactionCategoryService _transactionCategoryService;


        /// <summary>
        /// 
        /// </summary>
        public TransactionCategoryController(ITransactionCategoryService transactionCategoryService)
        {
            _transactionCategoryService = transactionCategoryService;
        }

        /// <summary>
        /// Adiciona uma categoria de transação.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Transactions-C" })]
        [HttpPost("Create")]
        public async Task<IActionResult> Add([FromQuery] CreateTransactionCategoryModel model) =>
             Result(await _transactionCategoryService.Add(model));


        /// <summary>
        /// Atualiza uma categoria de transação.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "TransactionCategories-E" })]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] UpdateTransactionCategoryModel model) =>
             Result(await _transactionCategoryService.Update(model));


        /// <summary>
        /// Retorna uma categoria de transação por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "TransactionCategories-V" })]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] long id) =>
             Result(await _transactionCategoryService.GetById(id));


        /// <summary>
        /// Retorna uma listagem de categorias.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "TransactionCategories-V" })]
        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] TransactionCategoryFilterModel model) =>
             Result(await _transactionCategoryService.GetPaginated(model));
    }
}
