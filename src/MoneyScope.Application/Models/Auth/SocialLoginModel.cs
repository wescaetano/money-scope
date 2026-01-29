using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Auth
{
    public class SocialLoginModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
    }
}
