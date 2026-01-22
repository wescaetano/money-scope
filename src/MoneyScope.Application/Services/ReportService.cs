using Microsoft.EntityFrameworkCore;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Report;
using MoneyScope.Core.Enums.Transaction;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IAuthService _authService;
        private readonly ITransactionService _transactionService;
        private readonly ICsvExportService _csvExportService;
        public ReportService(IRepositoryFactory repositoryFactory, IAuthService authService, ITransactionService transactionService, ICsvExportService csvExportService) : base(repositoryFactory)
        {
            _authService = authService;
            _transactionService = transactionService;
            _csvExportService = csvExportService;
        }

        public async Task<byte[]> GenerateMonthlyCsvAsync(long userId, int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var transactions = _repository<Transaction>().GetAllWithInclude(tr =>
                tr.UserId == userId &&
                tr.CreationDate >= startDate &&
                tr.CreationDate < endDate,
                i => i.Include(c => c.TransactionCategory)
            );

            var rows = transactions.Select(tx => new CsvTransactionRowModel
            {
                Date = tx.Date,
                Type = tx.Type.ToString(),
                Category = tx.TransactionCategory.Name,
                Description = tx.Description,
                Value = tx.Type == ETransactionType.Saida
                    ? -tx.Value
                    : tx.Value
            });

            return _csvExportService.Export(rows);
        }
        public async Task<ResponseModel<dynamic>> SendMonthlyReportAsync(long userId, int month, int year)
        {
            var user = await _repository<User>().Get(u => u.Id == userId);
            if (user == null) return FactoryResponse<dynamic>.NotFound("Usuario não encontrado!");


            var csv = await GenerateMonthlyCsvAsync(userId, month, year);

            await _authService.SendGenericEmail(
                user.Email,
                $"📊 Seu relatório financeiro - {month}/{year}",
                $"Olá, {user.Name},<br><br>",
                $"Segue em anexo o seu relatório financeiro mensal.<br><br>",
                $"— MoneyScope",
                new List<EmailAttachment>
                {
                    new()
                    {
                        FileName = $"relatorio-{month}-{year}.csv",
                        Content = csv,
                        ContentType = "text/csv"
                    }
                }
            );

            return FactoryResponse<dynamic>.Success("Relatório criado com sucesso!"); 
        }


        // Placeholder implementations for yearly report methods
        public async Task<ResponseModel<dynamic>> SendYearlyReportAsync(long userId, int year)
        {
            throw new NotImplementedException();
        }
        public async Task<byte[]> GenerateYearlyCsvAsync(long userId, int year)
        {
            throw new NotImplementedException();
        }
    }
}
