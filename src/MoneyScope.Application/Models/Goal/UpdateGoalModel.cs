using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Goal
{
    public class UpdateGoalModel
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Name { get; set; } = null!;
        public decimal? GoalValue { get; set; }
        public decimal? ActualValue { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
