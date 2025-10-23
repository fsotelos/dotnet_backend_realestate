using AutoMapper;
using MediatR;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Queries
{
    public class GetPropertyByIdHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto?>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetPropertyByIdHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<PropertyDto?> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.Id);

            if (property == null)
                return null;

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
