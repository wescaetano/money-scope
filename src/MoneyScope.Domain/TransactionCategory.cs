using MoneyScope.Core.Enums.TransactionCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Domain
{
    public class TransactionCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ETransactionCategoryType Type { get; set; }
        public long? UserId { get; set; }

        // Navigation properties
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
