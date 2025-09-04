using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dominicredit_api.Models;

namespace dominicredit_api.Interfaces
{
    public interface ItokenService
    {
        string CreateToken(AppUser user);
    }
}