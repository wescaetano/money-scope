using MoneyScope.Core.Enums.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Transaction
{
    public class CreateTransactionModel
    {
        [Required(ErrorMessage = "O campo 'userId' é obrigatório!")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "O campo 'transactionCategoryId' é obrigatório!")]
        public long TransactionCategoryId { get; set; }
        [Required(ErrorMessage = "O campo 'type' é obrigatório!")]
        public ETransactionType Type { get; set; }
        [Required(ErrorMessage = "O campo 'value' é obrigatório!")]
        public decimal Value { get; set; }
        [Required(ErrorMessage = "O campo 'date' é obrigatório!")]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
