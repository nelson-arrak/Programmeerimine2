using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, OperationResult<CustomerDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCustomerQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CustomerDto>();
            result.Value = await _dbContext.Customers
                .AsNoTracking()
                .Where(customer => customer.Id == request.Id)
                .Select(customer => new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Addresses = customer.Addresses.Select(address => new AddressDto
                    {
                        Id = address.Id,
                        Street = address.Street,
                        City = address.City,
                        State = address.State,
                        ZipCode = address.ZipCode,
                        Country = address.Country
                    }).ToList(),
                    Orders = customer.Orders
                        .OrderByDescending(order => order.OrderDate)
                        .Select(order => new CustomerOrderDto
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            Status = order.Status,
                            TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity)
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
