using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Models.Token;
using MoneyScope.Application.Models.User;
using MoneyScope.Core.Enums.User;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<dynamic>> Add(CreateUserModel model);
        Task<ResponseModel<dynamic>> Update(UpdateUserModel model);
        Task<ResponseModel<dynamic>> ChangeStatus(ChangeUserStatusModel model);
        Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(UserFilterModel filter);
        Task<ResponseModel<dynamic>> GetById(long id);
    }
}
