using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Account
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("User context is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            // match your JWT claim names
            var userId = user.FindFirst("uid")?.Value 
                         ?? throw new InvalidOperationException("User Id not found in token");

            var email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value 
                        ?? throw new InvalidOperationException("Email not found in token");

            var roles = user.Claims
                .Where(c => c.Type == "roles")
                .Select(c => c.Value);

            // optional custom claims if you add them later
            var nationality = user.FindFirst("Nationality")?.Value;
            var dateOfBirthString = user.FindFirst("DateOfBirth")?.Value;
            var dateOfBirth = dateOfBirthString == null
                ? (DateOnly?)null
                : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");

            return new CurrentUser(userId, email, roles);
        }
    }
}
