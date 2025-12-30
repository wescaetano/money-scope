using Microsoft.EntityFrameworkCore;
using MoneyScope.Application.Filters.TransactionCategory;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.TransactionCategory;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class TransactionCategoryService : BaseService, ITransactionCategoryService
    {
        public TransactionCategoryService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
        public async Task<ResponseModel<dynamic>> Add(CreateTransactionCategoryModel model)
        {
            var transactionCategory = new TransactionCategory
            {
                Name = model.Name,
                Type = model.Type,
                UserId = model.UserId,
                CreationDate = DateTime.Now
            };

            try
            {
                await _repository<TransactionCategory>().Create(transactionCategory);
                return FactoryResponse<dynamic>.SuccessfulCreation(description: "Categoria de transação criada com sucesso.");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno(description: $"Erro ao criar a categoria de transação: {ex.Message}");
            }
        }
        public async Task<ResponseModel<dynamic>> GetById(long id)
        {
            var transactionCategory = await _repository<TransactionCategory>().Get(id);
            if (transactionCategory == null) return FactoryResponse<dynamic>.NotFound(description: "Categoria de transação não encontrada.");

            return FactoryResponse<dynamic>.Success(transactionCategory, "Categoria de transação encontrada com sucesso!");
        }
        public async Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(TransactionCategoryFilterModel filter)
        {
            var transactionCategoriesQuery = _repository<TransactionCategory>().GetAllWithInclude(null, null);

            if (filter.Id != null) transactionCategoriesQuery = transactionCategoriesQuery.Where(x => x.Id == filter.Id);
            if (filter.Type != null) transactionCategoriesQuery = transactionCategoriesQuery.Where(x => x.Type == filter.Type);

            var filteredTransactionCategories = await transactionCategoriesQuery.ToListAsync();
            var total = filteredTransactionCategories.Count();
            //var prop = typeof(User).GetProperties().FirstOrDefault(x => x.Name.ToLower() == filter.SortField.ToLower());

            //if (filter.SortOrder.ToLower() != "desc")
            //{
            //    filteredUsers = filteredUsers.OrderBy(x => prop == null ? "Id" : prop.GetValue(x, null)).ToList();
            //}
            //else
            //{
            //    filteredUsers = filteredUsers.OrderByDescending(x => prop == null ? "Id" : prop.GetValue(x, null)).ToList();
            //}

            switch (filter.SortField?.ToLower())
            {
                case "name":
                    filteredTransactionCategories = filter.SortOrder?.ToLower() != "desc"
                        ? filteredTransactionCategories.OrderBy(x => x.Name).ToList()
                        : filteredTransactionCategories.OrderByDescending(x => x.Name).ToList();
                    break;

                default:
                    filteredTransactionCategories = filter.SortOrder?.ToLower() != "desc"
                        ? filteredTransactionCategories.OrderBy(x => x.Id).ToList()
                        : filteredTransactionCategories.OrderByDescending(x => x.Id).ToList();
                    break;
            }

            var retorno = filteredTransactionCategories;
            if (filter.Id == null && filter.PageNumber != null && filter.PageSize != null)
            {
                retorno = filteredTransactionCategories.Skip(filter.PageSize.Value * (filter.PageNumber.Value - 1)).Take(filter.PageSize.Value).ToList();
            }

            var transactionCategoriesRetrieve = retorno.Select(x => new
            {
                x.Id,
                x.Name,
                x.Type,
                x.UserId
            }).ToList();
            var paginationData = new PaginationData<dynamic>(transactionCategoriesRetrieve, total, filter);
            return FactoryResponse<PaginationData<dynamic>>.Success(paginationData);
        }
        public async Task<ResponseModel<dynamic>> Update(UpdateTransactionCategoryModel model)
        {
            var transactionCategory = await _repository<TransactionCategory>().Get(model.Id);
            if (transactionCategory == null) return FactoryResponse<dynamic>.NotFound(description: "Categoria de transação não encontrada.");

            if (model.UserId != null) transactionCategory.UserId = model.UserId;
            if (model.Name != null) transactionCategory.Name = model.Name;
            if (model.Type != null) transactionCategory.Type = model.Type.Value;
            transactionCategory.UpdateDate = DateTime.Now;

            try
            {
                await _repository<TransactionCategory>().Update(transactionCategory);
                return FactoryResponse<dynamic>.Success("Categoria de transação atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno($"Erro ao atualizar a categoria de transação: {ex.Message}");

            }
        }
    }
}
