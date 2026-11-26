using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    public class ListAddressesQueryHandler : IRequestHandler<ListAddressesQuery, OperationResult<PagedResult<AddressListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAddressesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<AddressListItemDto>>> Handle(ListAddressesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<AddressListItemDto>>();
            result.Value = await _dbContext.Addresses
                .AsNoTracking()
                .OrderBy(address => address.Id)
                .Select(address => new AddressListItemDto
                {
                    Id = address.Id,
                    Street = address.Street,
                    City = address.City,
                    Country = address.Country
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
