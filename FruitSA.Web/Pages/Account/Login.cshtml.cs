using FruitSA.Web.Models.Account;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitSA.Web.Pages.Account
{
    public class LoginModel(ApiService apiService) : PageModel
    {
        private readonly ApiService _apiService = apiService;

        [BindProperty]
        public LoginViewModel Login { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var payload = new
            {
                emailAddress = Login.Email,
                password = Login.Password
            };

            var response = await _apiService.PostAsync<JwtResponse>("api/Account/Login", payload);

            if (!string.IsNullOrEmpty(response?.Token))
            {
                _apiService.StoreToken(response.Token);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", response.Message ?? "Login failed. Please try again.");
            return Page();
        }
    }
}
