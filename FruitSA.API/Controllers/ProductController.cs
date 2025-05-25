using FruitSA.Application.Products.Commands.AddProduct;
using FruitSA.Application.Products.Commands.DeleteProduct;
using FruitSA.Application.Products.Commands.UpdateProduct;
using FruitSA.Application.Products.Queries.GetAllProductsByPagination;
using FruitSA.Application.Products.Queries.GetProductById;
using FruitSA.Application.Shared;
using FruitSA.Application.Shared.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductController(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost("Product")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request, CancellationToken cancellationToken)
        {
            var addProductCommand = new AddProductCommand()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                CreatedBy = _userManager.GetUserName(User)
            };
            var result = await _mediator.Send(addProductCommand, cancellationToken);
            if (!result.Success)
                return BadRequest(result.Message);

            return StatusCode(StatusCodes.Status201Created, result.Message);
        }


        [HttpGet("Products/Paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedProducts([FromQuery] PageModel pageModel, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllProductsByPaginationQuery(pageModel), cancellationToken);
            return Ok(result);
        }


        [HttpGet("Product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }


        [HttpPut("Product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ProductViewModel productViewModel, CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand()
            {
                Id = productViewModel.ProductId,
                CategoryId = productViewModel.CategoryId,
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Price = productViewModel.Price
            };
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success && result.Message == "Product not found.")
                return NotFound(result.Message);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }


        [HttpDelete("Product/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);
        }
    }
}
