using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookingTracker.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UsersModel> _logger;

        public UsersModel(UserManager<IdentityUser> userManager, ILogger<UsersModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public List<UserViewModel> Users { get; set; } = new();
        public string CurrentUserEmail { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string NewUserEmail { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewUserPassword { get; set; }

        [BindProperty]
        public bool NewUserIsAdmin { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public bool IsAdmin { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadUsersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateUserAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadUsersAsync();
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = NewUserEmail,
                Email = NewUserEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, NewUserPassword);

            if (result.Succeeded)
            {
                if (NewUserIsAdmin)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                _logger.LogInformation($"Admin created new user: {NewUserEmail}");
                StatusMessage = $"User {NewUserEmail} created successfully.";
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            await LoadUsersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Invalid user ID.";
                return RedirectToPage();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ErrorMessage = "User not found.";
                return RedirectToPage();
            }

            // Don't allow deleting yourself
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == userId)
            {
                ErrorMessage = "You cannot delete your own account.";
                return RedirectToPage();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Admin deleted user: {user.Email}");
                StatusMessage = $"User {user.Email} deleted successfully.";
            }
            else
            {
                ErrorMessage = "Failed to delete user.";
            }

            return RedirectToPage();
        }

        private async Task LoadUsersAsync()
        {
            CurrentUserEmail = User.Identity?.Name;
            var allUsers = _userManager.Users.ToList();

            Users = new List<UserViewModel>();
            foreach (var user in allUsers)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                Users.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = isAdmin
                });
            }
        }
    }
}
