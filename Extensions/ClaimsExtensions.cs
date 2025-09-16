using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dominicredit_api.Extensions
{
    public static class ClaimsExtensions
    {
public static string? GetUserName(this ClaimsPrincipal user)
{
    return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
}
        
    }
}