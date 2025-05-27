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
        public IFormFile ImageFile { get; set; }

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
            if (ImageFile != null)
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!validExtensions.Any(ext => ImageFile.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("ImageFile", "Only .jpg, .jpeg, or .png files are supported.");
                    await LoadCategoriesAsync();
                    return Page();
                }

                if (ImageFile.Length > 1 * 1024 * 1024) // 1MB limit
                {
                    ModelState.AddModelError("ImageFile", "Image file size must be less than 1MB.");
                    await LoadCategoriesAsync();
                    return Page();
                }

                // Convert image to Base64
                using var memoryStream = new MemoryStream();
                await ImageFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = ImageFile.ContentType; // e.g., image/jpeg
                Product.ImagePath = $"data:{mimeType};base64,{base64String}";
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
