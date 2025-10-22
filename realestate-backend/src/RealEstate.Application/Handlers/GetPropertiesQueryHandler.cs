using AutoMapper;
using MediatR;
using RealEstate.Application.DTOs;
using RealEstate.Application.Queries;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Handlers
{
    public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, PaginatedPropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedPropertyDto> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            var (properties, totalCount) = await _propertyRepository.GetFilteredAsync(
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