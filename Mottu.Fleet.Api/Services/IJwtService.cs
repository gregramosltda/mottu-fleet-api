using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}

