using API_Demo.Dto;
using API_Demo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
namespace API_Demo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerUserDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpiration);
            return Ok(result);
        }  

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginUserDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["RefreshToken"] ?? "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var result = await _authService.RefreshTokenAsync(userId, refreshToken);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("RevokeToken")]
        [Authorize]
        public async Task<IActionResult> RevokeTokenAsync([FromQuery] string? refreshToken)
        {
            var token = refreshToken ?? Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _authService.RevokeTokenAsync(userId, token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok("Revoke Token Successfully");
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _authService.ExternalResponse("Google");
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpGet("github-login")]
        public IActionResult GitHubLogin()
        {
            var redirectUrl = Url.Action(nameof(GitHubResponse), "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "GitHub");
        }

        [HttpGet("github-response")]
        public async Task<IActionResult> GitHubResponse()
        {
            var result = await _authService.ExternalResponse("GitHub");
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AddRoleDto addRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddToRole(addRoleDto);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok("Added to role Successfully");
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            bool result = await _authService.logoutAsync(userId);

            if (!result)
                return BadRequest("Error in Logout");


            Response.Cookies.Delete("RefreshToken"); 
            return Ok("Logout Successfully");
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expirationData)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expirationData.ToLocalTime(),
                SameSite = SameSiteMode.None,
            };
            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        }

    }
}



#region Using Contrroler 
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using API_Demo.Configuration;
//using API_Demo.Dto;
//using API_Demo.Model;

//namespace API_Demo.Controllers
//{
//    [ApiController]
//    [Route("[Controller]")]
//    public class AccountController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IOptions<JWTOptions> _options;
//        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWTOptions> options)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _options = options;
//        }

//        [HttpPost("Register")]
//        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = new ApplicationUser() { UserName = registerUserDto.UserName };
//                var result = await _userManager.CreateAsync(user, registerUserDto.Password);
//                if (result.Succeeded)
//                {
//                    return Ok("User Created Successfully");
//                }
//                return BadRequest(result.Errors);
//            }
//            return BadRequest(ModelState);
//        }


//        [HttpPost("Login")]
//        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
//                if (user != null && await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
//                {
//                    var token = await GenerateJwtToken(user);
//                    return Ok(new { token });
//                }
//                return Unauthorized();
//            }
//            return BadRequest("Invalid User Name Or Password");
//        }

//        private async Task<string> GenerateJwtToken(ApplicationUser user)
//        {
//            var roles = await _userManager.GetRolesAsync(user);
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name,user.UserName!),
//                new Claim(ClaimTypes.NameIdentifier,user.Id!),
//                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
//            };
//            foreach (var role in roles)
//                claims.Add(new Claim(ClaimTypes.Role, role));


//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var token = new JwtSecurityToken(
//                issuer: _options.Value.Issuer,
//                audience: _options.Value.Audience,
//                expires: DateTime.Now.AddMinutes(_options.Value.DurationInMinutes),
//                claims: claims,
//                signingCredentials: credentials
//                );
//            return new JwtSecurityTokenHandler().WriteToken(token);

//        }
//    }
//}

#endregion