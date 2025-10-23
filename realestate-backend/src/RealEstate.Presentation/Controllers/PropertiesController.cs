using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Queries;

namespace RealEstate.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropertyDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PropertyDto>> GetById(string id)
        {
            var query = new GetPropertyByIdQuery { Id = id };

            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginatedPropertyDto), 200)]
        public async Task<ActionResult<PaginatedPropertyDto>> Get(
            [FromQuery] string? name = null,
            [FromQuery] string? address = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10)
        {
            var query = new GetPropertiesQuery
            {
                Name = name,
                Address = address,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}