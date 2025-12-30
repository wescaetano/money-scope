using MoneyScope.Application.Filters.TransactionCategory;
using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Models.TransactionCategory;
using MoneyScope.Application.Models.User;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface ITransactionCategoryService
    {
        Task<ResponseModel<dynamic>> Add(CreateTransactionCategoryModel model);
        Task<ResponseModel<dynamic>> Update(UpdateTransactionCategoryModel model);
        Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(TransactionCategoryFilterModel filter);
        Task<ResponseModel<dynamic>> GetById(long id);
    }
}
