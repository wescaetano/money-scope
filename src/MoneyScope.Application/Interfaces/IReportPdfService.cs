using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{  
    public interface IReportPdfService
    {
        Task<byte[]> GenerateAndSendMonthlyReportAsync(long userId, int month, int year);
        Task<byte[]> GenerateMonthlyReportAsync(long userId, int month, int year);
        Task<ResponseModel<dynamic>> SendMonthlyReportsToAllUsersAsync();
    } 
}
