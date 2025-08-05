using Microsoft.AspNetCore.Identity;

namespace API_Demo.Model
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
