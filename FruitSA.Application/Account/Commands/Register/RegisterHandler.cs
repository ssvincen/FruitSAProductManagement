using FruitSA.Domain.Account;
using FruitSA.Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Account.Commands.Register
{
    public class RegisterHandler(UserManager<IdentityUser> userManager) : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;


        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new RegisterResponse
            {
                Success = false
            };
            var existingUser = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (existingUser != null)
            {
                response.Message = "Email address is already in use. Try resetting your password if you have forgotten it.";
                return response;
            }
            var user = new IdentityUser
            {
                UserName = request.EmailAddress,
                Email = request.EmailAddress
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.Message = string.Join("; ", result.Errors.Select(e => e.Description));
                return response;
            }
            await _userManager.AddToRoleAsync(user, UserRolesConst.Admin);

            //User is registered and can log in
            response.Success = true;
            response.Message = "Registration successful. Please check your email to confirm your account.";

            return response;
        }


    }
}
