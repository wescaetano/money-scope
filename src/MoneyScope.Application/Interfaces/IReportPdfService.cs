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
        Task<ResponseModel<dynamic>> GenerateMonthlyReportAsync(long userId, int month, int year
        );
    } 
}
