using MoneyScope.Core.Enums.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Domain
{
    public class Transaction : BaseEntity
    {
        public long UserId { get; set; }
        public long TransactionCategoryId { get; set; }
        public ETransactionType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }


        // Navigation properties
        public User User { get; set; } = null!;
        public TransactionCategory TransactionCategory { get; set; } = null!;
    }
}
