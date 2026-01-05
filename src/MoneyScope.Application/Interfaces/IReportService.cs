using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateMonthlyCsvAsync(long userId, int month, int year);
        Task<byte[]> GenerateYearlyCsvAsync(long userId, int year);

        Task<ResponseModel<dynamic>> SendMonthlyReportAsync(long userId, int month, int year);
        Task<ResponseModel<dynamic>> SendYearlyReportAsync(long userId, int year);
    }
}
