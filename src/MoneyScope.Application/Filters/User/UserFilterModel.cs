using MoneyScope.Core.Enums.User;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Filters.User
{
    public class UserFilterModel : FilterModel
    {
        public EUserStatus? Status { get; set; }
    }
}
