using FruitSA.Application.Shared.Category;
using FruitSA.Application.Shared.Product;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FruitSA.Web.Pages.Product
{
    public class EditModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;
        [BindProperty]
        public ProductViewModel Product { get; set; }

        public List<SelectListItem> Categories { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _apiService.GetAsync<ProductViewModel>($"api/Product/{id}");
            if (product == null)
                return NotFound();

            Product = product;
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
            if (ImageFile != null)
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!validExtensions.Any(ext => ImageFile.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("ImageFile", "Only .jpg, .jpeg, or .png files are supported.");
                    await LoadCategoriesAsync();
                    return Page();
                }

                if (ImageFile.Length > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image file size must be less than 1MB.");
                    await LoadCategoriesAsync();
                    return Page();
                }

                using var memoryStream = new MemoryStream();
                await ImageFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = ImageFile.ContentType;
                Product.ImagePath = $"data:{mimeType};base64,{base64String}";
            }

            var response = await _apiService.PutAsync<ApiResponse>($"api/Product", Product);
            if (response.Success)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
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
