using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    public class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, OperationResult<PagedResult<CustomerListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCustomersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<CustomerListItemDto>>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<CustomerListItemDto>>();
            result.Value = await _dbContext.Customers
                .AsNoTracking()
                .OrderBy(customer => customer.Name)
                .Select(customer => new CustomerListItemDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    IsAdmin = customer.IsAdmin,
                    OrderCount = customer.Orders.Count
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
