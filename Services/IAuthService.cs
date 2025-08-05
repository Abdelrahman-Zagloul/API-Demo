using API_Demo.Dto;

namespace API_Demo.Services
{
    public interface IAuthService
    {
        public Task<AuthenticationDto> RegisterAsync(RegisterUserDto dto);
        public Task<AuthenticationDto> LoginAsync(LoginUserDto dto);
        public Task<bool> logoutAsync(string userId);
        public Task<AuthenticationDto> RefreshTokenAsync(string UserId, string refreshToken);
        public Task<bool> RevokeTokenAsync(string UserId, string refreshToken);
        public Task<AuthenticationDto> ExternalResponse(string provider);
        public Task<string> AddToRole(AddRoleDto dto);


    }
}
