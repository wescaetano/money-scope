using MoneyScope.Application.Filters.Transaction;
using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Models.Transaction;
using MoneyScope.Application.Models.User;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ResponseModel<dynamic>> Add(CreateTransactionModel model);
        Task<ResponseModel<dynamic>> Update(UpdateTransactionModel model);
        Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(TransactionFilterModel filter);
        Task<ResponseModel<dynamic>> GetById(long id);
    }
}
