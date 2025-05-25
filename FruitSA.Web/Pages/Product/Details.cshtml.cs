using FruitSA.Application.Shared.Product;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Product
{
    public class DetailsModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;
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
    }
}
