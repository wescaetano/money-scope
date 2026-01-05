using MoneyScope.Core.Enums.Transaction;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Filters.Transaction
{
    public class TransactionFilterModel : FilterModel
    {
        public long? UserId { get; set; }
        public long? TransactionCategoryId { get; set; }
        public ETransactionType? Type { get; set; }
        public decimal? StartValue { get; set; }
        public decimal? EndValue { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
