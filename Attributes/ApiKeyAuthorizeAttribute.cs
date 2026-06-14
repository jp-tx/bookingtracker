using Microsoft.AspNetCore.Authorization;
using BookingTracker.Middleware;

namespace BookingTracker.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizeAttribute : AuthorizeAttribute
{
    public ApiKeyAuthorizeAttribute()
    {
        AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme;
    }
}
