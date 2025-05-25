using ClosedXML.Excel;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.Web.Pages.Product
{
    public class IndexModel(ApiService apiService) : SecurePageModel
    {
        private readonly ApiService _apiService = apiService;

        [BindProperty]
        public IFormFile UploadFile { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public PagedResult<ProductViewModel> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            string payload = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _apiService.GetAsync<PagedResult<ProductViewModel>>($"api/Product/Paged{payload}");

            if (response != null)
            {
                Products = response;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetDownloadExcelAsync()
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Products");

                // Header
                worksheet.Cell(1, 1).Value = "ProductId";
                worksheet.Cell(1, 2).Value = "CategoryName";
                worksheet.Cell(1, 3).Value = "Name";
                worksheet.Cell(1, 4).Value = "Description";
                worksheet.Cell(1, 5).Value = "Price";
                worksheet.Cell(1, 6).Value = "ProductCode";
                worksheet.Cell(1, 7).Value = "CreatedBy";
                worksheet.Cell(1, 8).Value = "CreatedDate";

                // Fetch products page by page
                int pageSize = 100; // Adjust based on performance testing
                int pageNumber = 1;
                int row = 2;
                while (true)
                {
                    string payload = $"?pageNumber={pageNumber}&pageSize={pageSize}";
                    var pageResult = await _apiService.GetAsync<PagedResult<ProductViewModel>>($"api/Product/Paged{payload}");
                    if (pageResult == null || !pageResult.Items.Any())
                    {
                        break;
                    }

                    foreach (var p in pageResult.Items)
                    {
                        worksheet.Cell(row, 1).Value = p.ProductId;
                        worksheet.Cell(row, 2).Value = p.CategoryName;
                        worksheet.Cell(row, 3).Value = p.Name;
                        worksheet.Cell(row, 4).Value = p.Description;
                        worksheet.Cell(row, 5).Value = p.Price;
                        worksheet.Cell(row, 6).Value = p.ProductCode;
                        worksheet.Cell(row, 7).Value = p.CreatedBy;
                        worksheet.Cell(row, 8).Value = p.CreatedDate.ToString("MM/dd/yyyy");
                        row++;
                    }
                    if (pageNumber >= pageResult.TotalPages)
                    {
                        break;
                    }
                    pageNumber++;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"Products_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                //Todo : Add logging
                return StatusCode(500, $"An error occurred while generating the Excel file. Error: {ex}");
            }
        }


        public async Task<IActionResult> OnPostUploadAsync()
        {
            try
            {
                if (UploadFile == null || UploadFile.Length == 0)
                {
                    ErrorMessage = "Please select a file to upload.";
                    return await LoadProductsAsync(1, 10);
                }

                // Validate file type and size
                if (!UploadFile.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Only .xlsx files are supported.";
                    return await LoadProductsAsync(1, 10);
                }

                if (UploadFile.Length > 10 * 1024 * 1024) // 10MB limit
                {
                    ErrorMessage = "File size exceeds 10MB limit.";
                    return await LoadProductsAsync(1, 10);
                }


                using var content = new MultipartFormDataContent();
                using var fileStream = UploadFile.OpenReadStream();
                content.Add(new StreamContent(fileStream), "File", UploadFile.FileName);

                var response = await _apiService.PostMultipartAsync<ApiResponse>("api/Product/Upload", content);
                if (response.Success)
                {
                    SuccessMessage = response.Message;
                }
                else
                {
                    ErrorMessage = "Failed to process the uploaded file.";
                }

                return await LoadProductsAsync(1, 10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                ErrorMessage = "An error occurred while uploading the file.";
                return await LoadProductsAsync(1, 10);
            }
        }

        private async Task<IActionResult> LoadProductsAsync(int pageNumber, int pageSize)
        {
            string payload = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _apiService.GetAsync<PagedResult<ProductViewModel>>($"api/Product/Paged{payload}");

            if (response != null)
            {
                Products = response;
            }

            return Page();
        }

    }
}
