using AutoMapper;
using MediatR;
using RealEstate.Application.DTOs;
using RealEstate.Application.Queries;
using RealEstate.Domain.Services;

namespace RealEstate.Application.Handlers
{
    public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, PaginatedPropertyDto>
    {
        private readonly IPropertyFilteringService _propertyFilteringService;
        private readonly IMapper _mapper;

        public GetPropertiesQueryHandler(IPropertyFilteringService propertyFilteringService, IMapper mapper)
        {
            _propertyFilteringService = propertyFilteringService;
            _mapper = mapper;
        }

        public async Task<PaginatedPropertyDto> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            var (properties, totalCount) = await _propertyFilteringService.GetFilteredPropertiesAsync(
                request.Name,
                request.Address,
                request.MinPrice,
                request.MaxPrice,
                request.Page ?? 1,
                request.PageSize ?? 10);

            return new PaginatedPropertyDto
            {
                Properties = _mapper.Map<IEnumerable<PropertyDto>>(properties),
                TotalCount = totalCount,
                Page = request.Page ?? 1,
                PageSize = request.PageSize ?? 10
            };
        }
    }
}