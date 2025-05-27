using FruitSA.Application.Interfaces;
using FruitSA.Application.Products.Commands.AddProduct;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using Moq;

namespace FruitSA.UnitTest.ProductTests
{
    public class AddProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;

        public AddProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
        }


        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResultWithProductViewModel()
        {
            // Arrange
            var handler = new AddProductHandler(_mockRepository.Object);
            //Base on SeedData 1 = fruit
            var command = new AddProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                CategoryId = 1,
                Price = 99.99m,
                CreatedBy = "TestUser"
            };

            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                CategoryId = command.CategoryId,
                Price = command.Price,
                ImagePath = command.ImagePath,
                CreatedBy = "System",
                CreatedDate = DateTime.UtcNow,
            };

            var expectedViewModel = new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                ImagePath = product.ImagePath
            };

            _mockRepository
                .Setup(repo => repo.AddProductAsync(It.Is<Product>(p =>
                    p.Name == command.Name &&
                    p.Description == command.Description &&
                    p.CategoryId == command.CategoryId &&
                    p.Price == command.Price &&
                    p.ImagePath == command.ImagePath &&
                    p.CreatedBy == command.CreatedBy), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductViewModel>.Ok(expectedViewModel));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedViewModel.ProductId, result.Data.ProductId);
            Assert.Equal(expectedViewModel.Name, result.Data.Name);
            Assert.Equal(expectedViewModel.Description, result.Data.Description);
            Assert.Equal(expectedViewModel.CategoryId, result.Data.CategoryId);
            Assert.Equal(expectedViewModel.Price, result.Data.Price);
            Assert.Equal(expectedViewModel.ImagePath, result.Data.ImagePath);

            _mockRepository.Verify(repo => repo.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_RepositoryFails_ReturnsFailureResult()
        {
            // Arrange
            var handler = new AddProductHandler(_mockRepository.Object);
            var command = new AddProductCommand
            {
                Name = "Invalid Product",
                Description = "Invalid Description",
                CategoryId = 1,
                Price = 50.00m,
                ImagePath = "/images/invalid.jpg",
                CreatedBy = "TestUser"
            };

            _mockRepository.Setup(repo => repo.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductViewModel>.Fail("Failed to add product to database"));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Failed to add product to database", result.Message);
            Assert.Null(result.Data);

            _mockRepository.Verify(repo => repo.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
