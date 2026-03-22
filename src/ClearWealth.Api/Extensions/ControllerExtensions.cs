using System.Security.Claims;

namespace ClearWealth.Api.Extensions;

public static class ControllerExtensions
{
    /// <summary>
    /// Extracts the authenticated user's ID from the JWT token's NameIdentifier claim.
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdValue))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        return Guid.Parse(userIdValue);
    }
}
