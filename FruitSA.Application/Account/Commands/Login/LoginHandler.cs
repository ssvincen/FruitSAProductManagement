using FruitSA.Application.Interfaces;
using FruitSA.Domain.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Account.Commands.Login
{
    public class LoginHandler(UserManager<IdentityUser> userManager, IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedAccessException("Account is locked. Try again later.");

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                await _userManager.AccessFailedAsync(user);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return new LoginResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(6)
            };
        }
    }
}
