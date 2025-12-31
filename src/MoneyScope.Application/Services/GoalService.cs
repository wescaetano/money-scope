using Microsoft.EntityFrameworkCore;
using MoneyScope.Application.Filters.Goal;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Goal;
using MoneyScope.Core.Enums.Goal;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class GoalService : BaseService, IGoalService
    {
        public GoalService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
        public async Task<ResponseModel<dynamic>> Add(CreateGoalModel model)
        {
            var goal = new Goal
            {
                UserId = model.UserId,
                Name = model.Name,
                GoalValue = model.GoalValue,
                ActualValue = model.ActualValue,
                Deadline = model.Deadline,
                Status = EGoalStatus.EmAndamento,
                CreationDate = DateTime.Now
            };

            try
            {
                await _repository<Goal>().Create(goal);
                return FactoryResponse<dynamic>.SuccessfulCreation("Meta criada com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno("Erro ao criar Meta.", ex);
            }
        }
        public async Task<ResponseModel<dynamic>> ChangeStatus(ChangeGoalStatusModel model)
        {
            var goal = await _repository<Goal>().Get(model.Id);
            if(goal == null) return FactoryResponse<dynamic>.NotFound("Meta não encontrada.");

            goal.Status = model.Status;
            
            try
            {
                await _repository<Goal>().Update(goal);
                return FactoryResponse<dynamic>.Success("Status da Meta atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno("Erro ao atualizar status da Meta.", ex);
            }
        }
        public async Task<ResponseModel<dynamic>> GetById(long id)
        {
            var goal = await _repository<Goal>().GetWithInclude(x => x.Id == id, i => i.Include(u => u.User));
            if (goal == null) return FactoryResponse<dynamic>.NotFound("Meta não encontrada.");

            var retrieve = new
            {
                goal.Id,
                goal.Name,
                goal.GoalValue,
                goal.ActualValue,
                goal.Deadline,
                goal.Status,
                User = new
                {
                    goal.User.Id,
                    goal.User.Name,
                    goal.User.Email
                }
            };

            return FactoryResponse<dynamic>.Success(retrieve, "Meta encontrada com sucesso!");
        }
        public async Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(GoalFilterModel filter)
        {
            var goalsQuery = _repository<Goal>().GetAllWithInclude(null, i => i.Include(u => u.User));

            if (filter.Id != null) goalsQuery = goalsQuery.Where(x => x.Id == filter.Id);
            if (filter.Name != null) goalsQuery = goalsQuery.Where(x => x.Name == filter.Name);
            if (filter.UserId != null) goalsQuery = goalsQuery.Where(x => x.UserId == filter.UserId);
            if (filter.GoalValue != null) goalsQuery = goalsQuery.Where(x => x.GoalValue == filter.GoalValue);
            if (filter.Deadline != null) goalsQuery = goalsQuery.Where(x => x.Deadline == filter.Deadline);
            if (filter.Status != null) goalsQuery = goalsQuery.Where(x => x.Status == filter.Status);
            

            var filteredGoals = await goalsQuery.ToListAsync();
            var total = filteredGoals.Count();

            switch (filter.SortField?.ToLower())
            {
                case "name":
                    filteredGoals = filter.SortOrder?.ToLower() != "desc"
                        ? filteredGoals.OrderBy(x => x.Name).ToList()
                        : filteredGoals.OrderByDescending(x => x.Name).ToList();
                    break;

                default:
                    filteredGoals = filter.SortOrder?.ToLower() != "desc"
                        ? filteredGoals.OrderBy(x => x.Id).ToList()
                        : filteredGoals.OrderByDescending(x => x.Id).ToList();
                    break;
            }

            var retrieve = filteredGoals;
            if (filter.Id == null && filter.PageNumber != null && filter.PageSize != null)
            {
                retrieve = filteredGoals.Skip(filter.PageSize.Value * (filter.PageNumber.Value - 1)).Take(filter.PageSize.Value).ToList();
            }

            var goalsRetrieve = retrieve.Select(x => new
            {
                x.Id,
                x.Name,
                x.GoalValue,
                x.ActualValue,
                x.Deadline,
                x.Status,
                User = new
                {
                    x.User.Id,
                    x.User.Name,
                    x.User.Email
                }
            }).ToList();

        var paginationData = new PaginationData<dynamic>(goalsRetrieve, total, filter);
            return FactoryResponse<PaginationData<dynamic>>.Success(paginationData);
        }
        public async Task<ResponseModel<dynamic>> Update(UpdateGoalModel model)
        {
            var goal = await _repository<Goal>().Get(x => x.Id == model.Id);
            if (goal == null) return FactoryResponse<dynamic>.NotFound("Meta não encontrada.");

            if(model.GoalValue != null) goal.GoalValue = model.GoalValue.Value;
            if(model.UserId != null) goal.UserId = model.UserId.Value;
            if(model.ActualValue != null) goal.ActualValue = model.ActualValue.Value;
            if(model.Name != null) goal.Name = model.Name;
            if(model.Deadline != null) goal.Deadline = model.Deadline.Value;
            goal.UpdateDate = DateTime.Now;
            try
            {
                await _repository<Goal>().Update(goal);
                return FactoryResponse<dynamic>.Success("Meta atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno("Erro ao atualizar Meta.", ex);
            }
        }
    }
}
