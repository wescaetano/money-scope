using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Goal
{
    public class CreateGoalModel
    {
        [Required(ErrorMessage = "O campo 'userId' é obrigatório!")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "O campo 'name' é obrigatório!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "O campo 'goalValue' é obrigatório!")]
        public decimal GoalValue { get; set; }
        [Required(ErrorMessage = "O campo 'actualValue' é obrigatório!")]
        public decimal ActualValue { get; set; }
        [Required(ErrorMessage = "O campo 'deadline' é obrigatório!")]
        public DateTime Deadline { get; set; }
    }
}
