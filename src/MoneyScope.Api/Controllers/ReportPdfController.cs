using Microsoft.AspNetCore.Mvc;
using MoneyScope.Application.Interfaces;
namespace MoneyScope.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class ReportPdfController : BaseController
    {
        private readonly IReportPdfService _reportPdfService;
        
        /// <summary>
        /// 
        /// </summary>
        public ReportPdfController(IReportPdfService reportPdfService)
        {
            _reportPdfService = reportPdfService;
        }
        /// <summary>
        /// Gera e envia por email o relatorio financeiro mensal em PDF.
        /// </summary>
        /// <param name="month">Mês do relatório.</param>
        /// <param name="year">Ano do relatório.</param>
        /// <returns></returns>
        [HttpPost("GenerateAndSendMonthlyReport")]
        public async Task<IActionResult> GenerateAndSendMonthlyReport([FromQuery] long userId, int month, int year) =>
             Result(await _reportPdfService.GenerateMonthlyReportAsync(userId, month, year));
    }
}
