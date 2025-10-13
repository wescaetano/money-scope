using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.User
{
    public class UpdateUserModel
    {
        public long Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? ImageUrl { get; set; }
        public long? AccessProfile { get; set; }
    }
}
