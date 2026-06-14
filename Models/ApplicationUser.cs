using Microsoft.AspNetCore.Identity;

namespace BookingTracker.Models;

public class ApplicationUser : IdentityUser
{
    public string? ApiKey { get; set; }
}
