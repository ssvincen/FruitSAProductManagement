using FruitSA.Application.Shared.Category;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Category
{
    public class CreateModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;

        [BindProperty]
        public AddCategoryRequest Category { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var request = new AddCategoryRequest
            {
                Name = Category.Name,
                Description = Category.Description,
                CategoryCode = Category.CategoryCode
            };

            var result = await _apiService.PostAsync<ApiResponse>("api/Category", request, true);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
