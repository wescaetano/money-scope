using MoneyScope.Application.Filters.Goal;
using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Models.Goal;
using MoneyScope.Application.Models.User;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface IGoalService
    {
        Task<ResponseModel<dynamic>> Add(CreateGoalModel model);
        Task<ResponseModel<dynamic>> Update(UpdateGoalModel model);
        Task<ResponseModel<dynamic>> ChangeStatus(ChangeGoalStatusModel model);
        Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(GoalFilterModel filter);
        Task<ResponseModel<dynamic>> GetById(long id);
    }
}
