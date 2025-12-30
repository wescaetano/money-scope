using MoneyScope.Core.Enums.TransactionCategory;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Filters.TransactionCategory
{
    public class TransactionCategoryFilterModel : FilterModel
    {
        public long? UserId { get; set; }
        public string? Name { get; set; }
        public ETransactionCategoryType? Type { get; set; }
    }
}
