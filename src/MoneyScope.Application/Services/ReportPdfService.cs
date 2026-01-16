using Microsoft.EntityFrameworkCore;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Report;
using MoneyScope.Application.Services;
using MoneyScope.Core.Enums.Transaction;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace Application.Services
{
    public class ReportPdfService : BaseService, IReportPdfService
    {
        private readonly ISendEmailService _sendEmailService;

        public ReportPdfService(
            IRepositoryFactory repositoryFactory,
            ISendEmailService sendEmailService
        ) : base(repositoryFactory)
        {
            _sendEmailService = sendEmailService;
        }

        public async Task<ResponseModel<dynamic>> GenerateMonthlyReportAsync(
            long userId,
            int month,
            int year
        )
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var user = await _repository<User>().Get(u => u.Id == userId);
            if (user == null)
                return FactoryResponse<dynamic>.NotFound("Usuário não encontrado!");

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var transactions =  _repository<Transaction>().GetAllWithInclude(t =>
                t.UserId == userId &&
                t.CreationDate >= startDate &&
                t.CreationDate < endDate, i => i.Include(tc => tc.TransactionCategory)
            );

            var pdf = GeneratePdf(transactions, month, year);

            await _sendEmailService.SendGenericEmail(
                user.Email,
                $"📊 Seu relatório financeiro - {month}/{year}",
                $"Olá, {user.Name},<br><br>",
                $"Segue em anexo o seu relatório financeiro mensal.<br><br>",
                $"— MoneyScope",
                new List<EmailAttachment>
                {
                    new()
                    {
                        FileName = $"relatorio-{month}-{year}.pdf",
                        Content = pdf,
                        ContentType = "application/pdf"
                    }
                }
            );

            return FactoryResponse<dynamic>.Success(
                "Relatório enviado para o seu e-mail com sucesso!"
            );
        }

        // =========================================================
        // PDF COM QUESTPDF
        // =========================================================
        private byte[] GeneratePdf(
    IEnumerable<Transaction> transactions,
    int month,
    int year
)
        {
            var culture = new CultureInfo("pt-BR");
            var totalSaidas = transactions.Where(t => t.Type == ETransactionType.Saida).Sum(t => t.Value);
            var totalEntradas = transactions.Where(t => t.Type == ETransactionType.Entrada).Sum(t => t.Value);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // ================= HEADER =================
                    page.Header().Column(header =>
                    {
                        header.Item().AlignCenter()
                            .Text("Relatório Financeiro Mensal")
                            .FontSize(20)
                            .Bold();

                        header.Item().AlignCenter()
                            .Text($"{culture.DateTimeFormat.GetMonthName(month)} / {year}")
                            .FontSize(12);

                        header.Item().PaddingVertical(10).LineHorizontal(1);
                    });

                    // ================= CONTENT =================
                    page.Content().Column(content =>
                    {
                        content.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Data
                                columns.RelativeColumn(2); // Tipo
                                columns.RelativeColumn(2); // Categoria
                                columns.RelativeColumn(3); // Descrição
                                columns.RelativeColumn(2); // Valor
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Data").Bold();
                                header.Cell().Element(CellStyle).Text("Tipo").Bold();
                                header.Cell().Element(CellStyle).Text("Categoria").Bold();
                                header.Cell().Element(CellStyle).Text("Descrição").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Valor").Bold();
                            });

                            foreach (var t in transactions)
                            {
                                table.Cell().Element(CellStyle)
                                    .Text(t.CreationDate?.ToString("dd/MM/yyyy") ?? "-");

                                table.Cell().Element(CellStyle)
                                    .Text(t.Type.ToString());

                                table.Cell().Element(CellStyle)
                                    .Text(t.TransactionCategory?.Name ?? "-");

                                table.Cell().Element(CellStyle)
                                    .Text(t.Description ?? "-");

                                table.Cell().Element(CellStyle)
                                    .AlignRight()
                                    .Text(t.Value.ToString("C", culture));
                            }
                        });

                        // ===== TOTAL =====
                        content.Item().PaddingTop(15).AlignRight().Text(text =>
                        {
                            text.Span("Total Saídas: ").Bold();
                            text.Span(totalSaidas.ToString("C", culture));
                        });

                        content.Item().PaddingTop(15).AlignRight().Text(text =>
                        {
                            text.Span("Total Entradas: ").Bold();
                            text.Span(totalEntradas.ToString("C", culture));
                        });
                    });

                    // ================= FOOTER =================
                    page.Footer().AlignCenter()
                        .Text($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Medium);
                });
            }).GeneratePdf();
        }


        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(6);
        }
    }
}
