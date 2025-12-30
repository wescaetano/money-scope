using MoneyScope.Application.Filters.Transaction;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Transaction;
using MoneyScope.Domain;
using MoneyScope.Core.Enums.Transaction;
using MoneyScope.Core.Models;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        public async Task<ResponseModel<dynamic>> Add(CreateTransactionModel model)
        {
            var transaction = new Transaction
            {
                UserId = model.UserId,
                TransactionCategoryId = model.TransactionCategoryId,
                Type = model.Type,
                Value = model.Value,
                Date = model.Date,
                Description = model.Description
            };

            try
            {
                await _repository<Transaction>().Create(transaction);
                return FactoryResponse<dynamic>.SuccessfulCreation("Transação criada com sucesso.");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno("Erro: " + ex.Message);
            }
        }
        public async Task<ResponseModel<dynamic>> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(TransactionFilterModel filter)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<dynamic>> Update(UpdateTransactionModel model)
        {
            throw new NotImplementedException();
        }
    }
}
