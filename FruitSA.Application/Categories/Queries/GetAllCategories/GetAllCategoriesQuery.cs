using FruitSA.Application.Shared.Category;
using MediatR;
using System.Collections.Generic;

namespace FruitSA.Application.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryViewModel>>
    {
    }
}
