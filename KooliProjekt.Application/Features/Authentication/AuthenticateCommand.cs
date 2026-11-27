using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authentication
{
    // Meie projektis on kasutajad Customers tabelis, sisselogimine käib e-posti järgi (Email on unikaalne)
    public class AuthenticateCommand : IRequest<OperationResult<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
