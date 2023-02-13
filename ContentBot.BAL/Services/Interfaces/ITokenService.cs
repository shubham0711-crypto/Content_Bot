using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(IEnumerable<Claim> claims);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    }
}
