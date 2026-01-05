using Microsoft.EntityFrameworkCore;
using MoneyScope.Application.Filters.Transaction;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Transaction;
using MoneyScope.Core.Enums.Transaction;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                Description = model.Description,
                CreationDate = DateTime.Now
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
            var transaction = await _repository<Transaction>().GetWithInclude(t => t.Id == id, i => i.Include(u => u.User).Include(tc => tc.TransactionCategory));
            if(transaction == null) return FactoryResponse<dynamic>.NotFound("Transação não encontrada.");

            var retrieve = new
            {
                transaction.Id,
                User = new
                {
                    transaction.User.Id,
                    transaction.User.Name,
                    transaction.User.Email
                },
                TransactionCategory = new
                {
                    transaction.TransactionCategory.Id,
                    transaction.TransactionCategory.Name
                },
                transaction.Type,
                transaction.Value,
                transaction.Date,
                transaction.Description
            };

            return FactoryResponse<dynamic>.Success(retrieve, "Transação recuperada com sucesso.");
        }
        public async Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(TransactionFilterModel filter)
        {
            var transactionQuery = _repository<Transaction>().GetAllWithInclude(null, i => i.Include(u => u.User).Include(tc => tc.TransactionCategory));

            if (filter.Id != null) transactionQuery = transactionQuery.Where(x => x.Id == filter.Id);
            if (filter.UserId != null) transactionQuery = transactionQuery.Where(x => x.UserId == filter.UserId);
            if (filter.TransactionCategoryId != null) transactionQuery = transactionQuery.Where(x => x.TransactionCategoryId == filter.TransactionCategoryId);
            if (filter.Type != null) transactionQuery = transactionQuery.Where(x => x.Type == filter.Type);
            if (filter.StartValue != null && filter.EndValue == null) transactionQuery = transactionQuery.Where(x => x.Value >= filter.StartValue);
            if (filter.StartValue == null && filter.EndValue != null) transactionQuery = transactionQuery.Where(x => x.Value <= filter.EndValue);
            if (filter.StartValue != null && filter.EndValue != null) transactionQuery = transactionQuery.Where(x => x.Value >= filter.StartValue && x.Value <= filter.EndValue);
            if (filter.Month != null && filter.Year != null)
            {
                if(filter.Month == 0 || filter.Month > 12) return FactoryResponse<PaginationData<dynamic>>.BadRequest("Mês inválido.");

                var startDate = new DateTime(filter.Year.Value, filter.Month.Value, 1);
                var endDate = startDate.AddMonths(1);
                transactionQuery = transactionQuery.Where(x => x.Date >= startDate && x.Date < endDate);
            };

            var filteredTransactions = await transactionQuery.ToListAsync();
            var total = filteredTransactions.Count();

            switch (filter.SortField?.ToLower())
            {
                default:
                    filteredTransactions = filter.SortOrder?.ToLower() != "desc"
                        ? filteredTransactions.OrderBy(x => x.Id).ToList()
                        : filteredTransactions.OrderByDescending(x => x.Id).ToList();
                    break;
            }

            var retorno = filteredTransactions;
            if (filter.Id == null && filter.PageNumber != null && filter.PageSize != null)
            {
                retorno = filteredTransactions.Skip(filter.PageSize.Value * (filter.PageNumber.Value - 1)).Take(filter.PageSize.Value).ToList();
            }

            var transactionsRetrieve = retorno.Select(x => new
            {
                x.Id,
                User = new
                {
                    x.User.Id,
                    x.User.Name,
                    x.User.Email
                },
                TransactionCategory = new
                {
                    x.TransactionCategory.Id,
                    x.TransactionCategory.Name
                },
                x.Type,
                x.Value,
                x.Date,
                x.Description
            }).ToList();
        var paginationData = new PaginationData<dynamic>(transactionsRetrieve, total, filter);
            return FactoryResponse<PaginationData<dynamic>>.Success(paginationData);
        }
        public async Task<ResponseModel<dynamic>> Update(UpdateTransactionModel model)
        {
            var transaction = await _repository<Transaction>().Get(model.Id);
            if (transaction == null) return FactoryResponse<dynamic>.NotFound("Transação não encontrada.");

            if(model.TransactionCategoryId != null) transaction.TransactionCategoryId = model.TransactionCategoryId.Value;
            if(model.Type != null) transaction.Type = model.Type.Value;
            if(model.Value != null) transaction.Value = model.Value.Value;
            if(model.Date != null) transaction.Date = model.Date.Value;
            if(model.Description != null) transaction.Description = model.Description;
            transaction.UpdateDate = DateTime.Now;
            
            try
            {
                await _repository<Transaction>().Update(transaction);
                return FactoryResponse<dynamic>.Success("Transação atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno("Erro: " + ex.Message);
            }
        }
    }
}
