using FruitSA.Domain.Account;
using FruitSA.Web.Models.Account;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitSA.Web.Pages.Account
{
    public class RegisterModel(ApiService apiService) : PageModel
    {
        private readonly ApiService _apiService = apiService;

        [BindProperty]
        public RegisterViewModel Register { get; set; }


        [TempData]
        public string FeedbackMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var payload = new
            {
                emailAddress = Register.Email,
                password = Register.Password,
                confirmPassword = Register.ConfirmPassword
            };

            var response = await _apiService.PostAsync<RegisterResponse>("api/Account/Register", payload);

            if (response.Success)
            {
                FeedbackMessage = response.Message;
                //FeedbackMessage = "Registration successful! You can now log in.";
                return RedirectToPage("/Account/Login");
            }
            ModelState.AddModelError(string.Empty, response.Message ?? "Registration failed. Please try again.");
            return Page();
        }
    }
}
