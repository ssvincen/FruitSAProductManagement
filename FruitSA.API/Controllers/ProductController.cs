using ClosedXML.Excel;
using FruitSA.Application.Categories.Queries.GetAllCategories;
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
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request, CancellationToken cancellationToken)
        {
            var addProductCommand = new AddProductCommand()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                CreatedBy = _userManager.GetUserName(User),
                ImagePath = request.ImagePath,
                Price = request.Price
            };
            var result = await _mediator.Send(addProductCommand, cancellationToken);
            if (!result.Success)
                return BadRequest(result);

            return StatusCode(StatusCodes.Status201Created, result);
        }


        [HttpGet("Paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedProducts([FromQuery] PaginationModel pagination, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllProductsByPaginationQuery(pagination), cancellationToken);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            if (!result.Success)
                return NotFound(result);

            return Ok(result.Data);
        }


        [HttpPut]
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
                Price = productViewModel.Price,
                ImagePath = productViewModel.ImagePath
            };
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success && result.Message == "Product not found.")
                return NotFound(result);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest fileUpload, CancellationToken cancellationToken)
        {
            if (fileUpload.File == null || fileUpload.File.Length == 0)
                return BadRequest("No file uploaded.");

            if (!fileUpload.File.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only .xlsx files are supported.");

            try
            {
                var categories = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
                var categoryDict = categories.ToDictionary(c => c.Name.Trim().ToLower(), c => c.CategoryId);

                using var stream = fileUpload.File.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet("Products");

                var products = new List<ProductViewModel>();
                //Assume the first row is the header skip it
                var rows = worksheet.RowsUsed().Skip(1);
                var errors = new List<string>();

                foreach (var row in rows)
                {
                    try
                    {
                        var categoryName = row.Cell(2).GetString()?.Trim();
                        var normalizedCategoryName = categoryName?.Trim().ToLower();

                        if (string.IsNullOrEmpty(categoryName) || !categoryDict.TryGetValue(normalizedCategoryName, out int categoryId))
                        {
                            errors.Add($"Row {row.RowNumber()}: Invalid or missing CategoryName '{normalizedCategoryName}'.");
                            continue;
                        }

                        var product = new ProductViewModel
                        {
                            Name = row.Cell(3).GetString()?.Trim(),
                            Description = row.Cell(4).GetString()?.Trim(),
                            Price = row.Cell(5).GetValue<decimal>(),
                            ProductCode = row.Cell(6).GetString()?.Trim(),
                            CategoryName = normalizedCategoryName,
                            CategoryId = categoryId,

                        };

                        // Basic validation
                        if (string.IsNullOrEmpty(product.Name))
                        {
                            errors.Add($"Row {row.RowNumber()}: Name and ProductCode are required.");
                            continue;
                        }
                        if (!decimal.TryParse(product.Price.ToString(), out var price) || price < 0)
                        {
                            errors.Add("Invalid Price");
                            continue;
                        }
                        product.Price = price;
                        products.Add(product);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Row {row.RowNumber()}: Error processing row - {ex.Message}");
                    }
                }
                if (errors.Any())
                {
                    return BadRequest(new { Message = "Some rows could not be processed.", Errors = errors });
                }
                foreach (var product in products)
                {
                    var command = new AddProductCommand
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        CreatedBy = _userManager.GetUserName(User)
                    };
                    await _mediator.Send(command, cancellationToken);
                }

                return Ok(new { Message = $"Processed {products.Count} products successfully.", Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to process file: {ex.Message}");
            }
        }
    }
}
