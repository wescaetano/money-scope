using MoneyScope.Core.Enums.Transaction;

namespace MoneyScope.Api.Extensions
{
    public static class TransactionTypeExtensions
    {
        public static string ToDisplay(this ETransactionType type)
        {
            return type switch
            {
                ETransactionType.Entrada => "Entrada",
                ETransactionType.Saida => "Saída",
                _ => "-"
            };
        }
    }
}
