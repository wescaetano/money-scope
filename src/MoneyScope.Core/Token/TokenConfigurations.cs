using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Core.Token
{
    public class TokenConfigurations
    {
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public int Seconds { get; set; }
    }
}
