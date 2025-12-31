using MoneyScope.Core.Enums.Goal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Domain
{
    public class Goal : BaseEntity
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public decimal GoalValue { get; set; }
        public decimal ActualValue { get; set; }
        public DateTime  Deadline { get; set; }
        public EGoalStatus  Status { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }
}
