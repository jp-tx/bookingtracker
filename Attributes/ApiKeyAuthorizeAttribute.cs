using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookingTracker.Middleware;

namespace BookingTracker.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizeAttribute : AuthorizeAttribute
{
    public ApiKeyAuthorizeAttribute()
    {
        // Accept both cookie authentication (Identity.Application) and API key authentication
        AuthenticationSchemes = $"{IdentityConstants.ApplicationScheme},{ApiKeyAuthenticationOptions.DefaultScheme}";
    }
}
