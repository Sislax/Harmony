using Microsoft.AspNetCore.Identity;

namespace Harmony.Infrastructure.Identity
{
    /// <summary>
    /// ApplicationUser class that inherits from IdentityUser. It is used to handle user authentication and authorization.
    /// It is different from User which is used in the Domain layer to represent a user entity.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
