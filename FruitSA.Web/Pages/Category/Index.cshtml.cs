using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Category
{
    public class IndexModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;

        public PagedResult<CategoryViewModel> Categories { get; set; }
        public async Task<IActionResult> OnGet()
        {
            int pageNumber = 1;
            int pageSize = 10;

            string payload = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _apiService.GetAsync<PagedResult<CategoryViewModel>>($"api/Category/Paged{payload}");

            if (response != null)
            {
                Categories = response;
            }
            return Page();
        }
    }
}
