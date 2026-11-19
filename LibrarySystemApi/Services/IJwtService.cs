using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Services;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}
