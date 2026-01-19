using Microsoft.AspNetCore.Mvc;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.User;
using MoneyScope.Application.Services;
using MoneyScope.Core.Models;
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
       
        [HttpGet("relatorio-mensal")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateMonthlyReport(
            [FromQuery] long userId,
            [FromQuery] int month,
            [FromQuery] int year
        )
        {
            try
            {
                var pdf = await _reportPdfService.GenerateMonthlyReportAsync(userId, month, year);

                return File(
                    pdf,
                    "application/pdf",
                    $"relatorio-{month}-{year}.pdf"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("enviar-relatorio-mensal")]
        [ProducesResponseType(typeof(ResponseModel<dynamic>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateAndSendMonthlyReport(
            [FromQuery] long userId,
            [FromQuery] int month,
            [FromQuery] int year
        )
        {
            try
            {
                var pdf = await _reportPdfService.GenerateAndSendMonthlyReportAsync(userId, month, year);

                // Retorna sucesso + opção de download
                return File(
                    pdf,
                    "application/pdf",
                    $"relatorio-{month}-{year}.pdf"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Adiciona um usuario.
        /// </summary>
        /// <returns></returns>
        //[APIAuthorization(new string[] { "Users-C" })]
        [HttpPost("BackgroundTest")]
        public async Task<IActionResult> Add() =>
             Result(await _reportPdfService.SendMonthlyReportsToAllUsersAsync());
    }
}
