using FruitSA.Application.Interfaces;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler(ICategoryRepository categoryRepository)
        : IRequestHandler<DeleteCategoryCommand, Result<bool>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.DeleteCategoryByIdAsync(request.Id, cancellationToken);
        }
    }
}
