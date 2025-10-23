using AutoMapper;
using MediatR;
using RealEstate.Application.DTOs;
using RealEstate.Application.Handlers;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Queries
{
    public class GetPropertyByIdQuery : IRequest<PropertyDto>
    {
        public string? Id { get; set; } = null;
    }
}
