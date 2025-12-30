using MoneyScope.Core.Enums.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Transaction
{
    public class UpdateTransactionModel
    {
        public long? UserId { get; set; }
        public long? TransactionCategoryId { get; set; }
        public ETransactionType? Type { get; set; }
        public decimal? Value { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
    }
}
