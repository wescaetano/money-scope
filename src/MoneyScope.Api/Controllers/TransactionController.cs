using Microsoft.AspNetCore.Mvc;
using MoneyScope.Api.Authorization;
using MoneyScope.Application.Filters.Transaction;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Transaction;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;

namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        /// <summary>
        /// 
        /// </summary>
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        /// <summary>
        /// Adiciona uma transação.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Transactions-C" })]
        [HttpPost("Create")]
        public async Task<IActionResult> Add([FromQuery] CreateTransactionModel model) =>
             Result(await _transactionService.Add(model));


        /// <summary>
        /// Altera uma transação.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Transactions-E" })]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] UpdateTransactionModel model) =>
             Result(await _transactionService.Update(model));


        /// <summary>
        /// Retorna uma transação por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Transactions-V" })]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] long id) =>
             Result(await _transactionService.GetById(id));


        /// <summary>
        /// Retorna uma listagem de transações.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [APIAuthorization(new string[] { "Transactions-V" })]
        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] TransactionFilterModel model) =>
             Result(await _transactionService.GetPaginated(model));
    }
}
