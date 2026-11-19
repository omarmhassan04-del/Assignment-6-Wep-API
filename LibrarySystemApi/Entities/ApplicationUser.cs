using Microsoft.AspNetCore.Identity;

namespace LibrarySystemApi.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}