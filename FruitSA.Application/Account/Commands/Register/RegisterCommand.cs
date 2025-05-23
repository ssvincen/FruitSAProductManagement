using FruitSA.Domain.Account;
using MediatR;

namespace FruitSA.Application.Account.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
