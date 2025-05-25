using FruitSA.Application.Shared.Category;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Category
{
    public class DetailsModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;

        public CategoryViewModel Category { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var category = await _apiService.GetAsync<CategoryViewModel>($"api/Category/{id}");
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }
    }
}
