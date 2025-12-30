using MoneyScope.Core.Enums.TransactionCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.TransactionCategory
{
    public class UpdateTransactionCategoryModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public ETransactionCategoryType? Type { get; set; }
        public long? UserId { get; set; }
    }
}
