using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<BalanceProductDto>), 200)]
        public async Task<ActionResult<List<BalanceProductDto>>> GetProducts()
        {
            var query = new GetProductsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BalanceProductDto), 201)]
        public async Task<ActionResult<BalanceProductDto>> CreateProduct(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProducts), new { id = result.Id }, result);
        }
    }
} 