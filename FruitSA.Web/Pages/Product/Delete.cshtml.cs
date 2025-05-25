using FruitSA.Application.Shared.Product;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Product
{
    public class DeleteModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;
        [BindProperty]
        public ProductViewModel Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _apiService.GetAsync<ProductViewModel>($"api/Product/{id}");
            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var response = await _apiService.DeleteAsync<ApiResponse>($"api/Product/{id}");
            if (response.Success)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete product.");
            return Page();
        }
    }
}
