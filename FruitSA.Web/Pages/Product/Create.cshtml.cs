using FruitSA.Application.Shared.Category;
using FruitSA.Application.Shared.Product;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FruitSA.Web.Pages.Product
{
    public class CreateModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;

        [BindProperty]
        public AddProductRequest Product { get; set; } = new AddProductRequest();

        public List<SelectListItem> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return Page();
            }

            var response = await _apiService.PostAsync<ApiResponse>("api/Product", Product, true);
            if (response.Success)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create product.");
            await LoadCategoriesAsync();
            return Page();
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _apiService.GetAsync<List<CategoryViewModel>>("api/Category");
            Categories = categories?.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList() ?? new List<SelectListItem>();
        }
    }
}
