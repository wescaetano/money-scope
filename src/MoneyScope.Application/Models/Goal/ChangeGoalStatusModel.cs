using MoneyScope.Core.Enums.Goal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Goal
{
    public class ChangeGoalStatusModel
    {
        public long Id { get; set; }
        public EGoalStatus Status { get; set; }
    }
}
