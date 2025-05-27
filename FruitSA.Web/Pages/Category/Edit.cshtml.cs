using FruitSA.Application.Shared.Category;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Category
{
    public class EditModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;
        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _apiService.PutAsync<ApiResponse>("api/Category", Category);
            if (response.Success)
            {
                return RedirectToPage("./Index");
            }
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }
    }
}
