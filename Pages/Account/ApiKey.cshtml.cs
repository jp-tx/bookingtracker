using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTracker.Models;

namespace BookingTracker.Pages.Account
{
    [Authorize]
    public class ApiKeyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApiKeyModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string? CurrentApiKey { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            CurrentApiKey = user.ApiKey;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            // Generate new API key
            user.ApiKey = Guid.NewGuid().ToString("N");
            await _userManager.UpdateAsync(user);

            StatusMessage = "Your API key has been reset successfully.";
            return RedirectToPage();
        }
    }
}
