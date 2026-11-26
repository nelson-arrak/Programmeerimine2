using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, OperationResult<AddressDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAddressQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<AddressDto>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AddressDto>();
            result.Value = await _dbContext.Addresses
                .AsNoTracking()
                .Where(address => address.Id == request.Id)
                .Select(address => new AddressDto
                {
                    Id = address.Id,
                    Street = address.Street,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode,
                    Country = address.Country
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
