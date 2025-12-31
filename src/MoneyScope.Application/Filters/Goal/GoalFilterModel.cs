using MoneyScope.Core.Enums.Goal;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Filters.Goal
{
    public class GoalFilterModel : FilterModel
    {
        public long? UserId { get; set; }
        public string? Name { get; set; } = null!;
        public decimal? GoalValue { get; set; }
        public DateTime? Deadline { get; set; }
        public EGoalStatus? Status { get; set; }
    }
}
