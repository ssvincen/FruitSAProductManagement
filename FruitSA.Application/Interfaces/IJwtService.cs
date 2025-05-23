using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FruitSA.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(IdentityUser identityUser, IEnumerable<string> roles);
    }
}
