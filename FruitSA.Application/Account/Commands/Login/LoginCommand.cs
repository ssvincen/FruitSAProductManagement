using FruitSA.Domain.Account;
using MediatR;

namespace FruitSA.Application.Account.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
