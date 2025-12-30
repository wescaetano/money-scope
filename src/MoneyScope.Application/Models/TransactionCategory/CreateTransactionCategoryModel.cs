using MoneyScope.Core.Enums.TransactionCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.TransactionCategory
{
    public class CreateTransactionCategoryModel
    {
        [Required(ErrorMessage = "O campo 'name' é obrigatório!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "O campo 'type' é obrigatório!")]
        public ETransactionCategoryType Type { get; set; }
        public long? UserId { get; set; }
    }
}
