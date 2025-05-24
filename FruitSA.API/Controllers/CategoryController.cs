using FruitSA.Application.Categories.Commands.AddCategory;
using FruitSA.Application.Shared.Category;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.API.Controllers
{
    //[Authorize]
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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request, CancellationToken cancellationToken)
        {
            var addCategoriesCommand = new AddCategoryCommand()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryCode = request.CategoryCode,
                CreatedBy = _userManager.GetUserId(User)
            };

            var result = await _mediator.Send(addCategoriesCommand, cancellationToken);

            if (!result.Success)
                return BadRequest(result.Message);

            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
