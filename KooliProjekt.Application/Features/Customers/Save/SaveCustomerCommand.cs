using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Customers
{
    public class SaveCustomerCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Uue kliendi puhul kohustuslik, muutmisel võib tühjaks jätta (siis parool ei muutu)
        public string Password { get; set; }
    }
}
