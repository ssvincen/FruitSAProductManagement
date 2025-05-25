using FruitSA.Application.Categories.Commands.AddCategory;
using FruitSA.Application.Categories.Commands.DeleteCategory;
using FruitSA.Application.Categories.Commands.UpdateCategory;
using FruitSA.Application.Categories.Queries.GetAllCategories;
using FruitSA.Application.Categories.Queries.GetAllCategoriesByPagination;
using FruitSA.Application.Categories.Queries.GetCategoryById;
using FruitSA.Application.Shared;
using FruitSA.Application.Shared.Category;
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
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }


        [HttpGet("Categories/Paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoriesByPagination([FromQuery] PageModel pageModel, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllCategoriesByPaginationQuery(pageModel), cancellationToken);
            return Ok(result);
        }


        [HttpPost("Category")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request, CancellationToken cancellationToken)
        {
            var addCategoriesCommand = new AddCategoryCommand()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryCode = request.CategoryCode,
                CreatedBy = _userManager.GetUserName(User)
            };

            var result = await _mediator.Send(addCategoriesCommand, cancellationToken);

            if (!result.Success)
                return BadRequest(result.Message);

            return StatusCode(StatusCodes.Status201Created, result);
        }


        [HttpGet("Categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
            return Ok(result);
        }


        [HttpGet("Category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }



        [HttpPut("Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryViewModel request, CancellationToken cancellationToken)
        {
            var command = new UpdateCategoryCommand
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                CategoryCode = request.CategoryCode,
                IsActive = request.IsActive
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }



        [HttpDelete("Categories/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryCommand { Id = id };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }



    }
}
