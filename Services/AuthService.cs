using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Demo.Dto;
using API_Demo.Model;
using API_Demo.Settings;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using API_Demo.Data;
using Twilio.TwiML.Voice;
namespace API_Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwt;
        private readonly AppDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwt, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<AuthenticationDto> RegisterAsync(RegisterUserDto dto)
        {
            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                return new AuthenticationDto { Message = "User Name is already exist!" };

            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return new AuthenticationDto { Message = "Email is already exist!" };

            var user = new ApplicationUser() { Email = dto.Email, UserName = dto.UserName };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("\n", result.Errors.Select((x, i) => $"{i + 1}. {x.Description}"));
                return new AuthenticationDto() { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");


            var jwtToken = await CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user);
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
            return new AuthenticationDto
            {
                ExpiresOn = jwtToken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                IsAuthenticated = true,
                UserName = user.UserName,
                Roles = new List<string> { "User" },
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn,
            };
        }

        public async Task<AuthenticationDto> LoginAsync(LoginUserDto dto)
        {
            var authDto = new AuthenticationDto();
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                authDto.Message = "Username Or Password is incorrect!";
                return authDto;
            }

            var jwtToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);


            authDto.ExpiresOn = jwtToken.ValidTo;
            authDto.IsAuthenticated = true;
            authDto.Roles = roles.ToList();
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authDto.UserName = user.UserName ?? "Unknow";

            var refreshTokens = _context.RefreshTokens.Where(x => x.ApplicationUserId == user.Id).ToList();
            if (refreshTokens.Any(x => x.IsActive))
            {
                var activeRefreshToken = refreshTokens.FirstOrDefault(t => t.IsActive);
                authDto.RefreshToken = activeRefreshToken?.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken!.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken(user);
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                _context.RefreshTokens.Add(refreshToken);
                _context.SaveChanges();
            }

            return authDto;

        }
        public async Task<AuthenticationDto> RefreshTokenAsync(string userId, string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new AuthenticationDto() { Message = "User Not Exist" };

            var RefreshToken = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken && t.ApplicationUserId == user.Id);

            if (RefreshToken is null)
                return new AuthenticationDto() { Message = "Invalid Refresh Token" };
            if (!RefreshToken.IsActive)
                return new AuthenticationDto() { Message = "Inactive Refresh Token" };

            RefreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken(user);
            _context.RefreshTokens.Add(newRefreshToken);
            _context.SaveChanges();

            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthenticationDto()
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = token.ValidTo,
                Roles = roles.ToList(),
                UserName = user.UserName ?? "NA",
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn,
            };
        }
        public async Task<bool> RevokeTokenAsync(string userId, string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;
            var RefreshToken = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken /*&& t.ApplicationUserId == user.Id*/);

            if (RefreshToken is null)
                return false;

            if (!RefreshToken.IsActive)
                return false;

            RefreshToken.RevokedOn = DateTime.UtcNow;
            _context.RefreshTokens.Update(RefreshToken);
            _context.SaveChanges();
            return true;
        }
        public async Task<AuthenticationDto> ExternalResponse(string provider)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return new AuthenticationDto { Message = "No HttpContext found" };

            var result = await httpContext.AuthenticateAsync(provider);
            if (!result.Succeeded)
                return new AuthenticationDto() { Message = $"{provider} login failed" };

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ??
                        result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "Unknow";

            if (string.IsNullOrEmpty(email))
                return new AuthenticationDto() { Message = $"No email returned from {provider}" };

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
            }

            var jwtToken = await CreateJwtToken(user);
            return new AuthenticationDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                IsAuthenticated = true,
                UserName = user.UserName ?? "Unknow",
                ExpiresOn = jwtToken.ValidTo,
            };
        }
        public async Task<string> AddToRole(AddRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
                return "User is not exist";
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                return "Role is not exist";

            bool hasRole = await _userManager.IsInRoleAsync(user, dto.Role);
            if (hasRole)
                return $"User is already assign to '{dto.Role}' role";
            var result = await _userManager.AddToRoleAsync(user, dto.Role);

            return result.Succeeded ? string.Empty : "error will add role";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            if (user == null)
                return new JwtSecurityToken();

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email??""),
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName??""),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));



            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
                claims: claims,
                signingCredentials: signingCredentials
                );
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(ApplicationUser user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                ApplicationUserId = user.Id,
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
            };

        }

    }
}
