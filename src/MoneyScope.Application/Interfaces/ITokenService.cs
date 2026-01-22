using MoneyScope.Application.Models.Token;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface ITokenService
    {
        Task<ResponseModel<dynamic>?> ValidateRefreshToken(string token);
        Task<string> GenerateRefreshToken(long userId);
        Task<ResponseModel<dynamic>> GenerateToken(GenerateTokenModel model);
        Task<ResponseModel<dynamic>> GetToken(User user, int? seconds = null);
        Task<ResponseModel<dynamic>> GenerateTokenByEmail(string email);
        bool IsValidToken(string token);
        string? Getclaim(string claim, string token);
    }
}
