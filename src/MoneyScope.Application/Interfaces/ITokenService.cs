using MoneyScope.Application.Models.Token;
using MoneyScope.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface ITokenService
    {
        Task<ResponseModel<dynamic>> GerenerateToken(GenerateTokenModel model);
        Task<ResponseModel<dynamic>> GenerateTokenByEmail(string email);
        bool IsValidToken(string token);
        string? Getclaim(string claim, string token);
    }
}
