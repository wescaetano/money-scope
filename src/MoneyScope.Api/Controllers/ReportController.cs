using Microsoft.AspNetCore.Mvc;
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
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        /// <summary>
        /// 
        /// </summary>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Envia o relatório mensal por email.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendReport([FromQuery] long userId, int month, int year) =>
             Result(await _reportService.SendMonthlyReportAsync(userId, month, year));
    }
}
