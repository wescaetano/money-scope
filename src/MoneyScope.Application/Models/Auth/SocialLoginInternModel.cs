using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.Auth
{
    public class SocialLoginInternModel
    {
        public string Login { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
    }
}
