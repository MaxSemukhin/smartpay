using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
    private readonly ApplicationDbContext _db;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IOptions<ApiBehaviorOptions> apiBehaviorOptions, ApplicationDbContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _apiBehaviorOptions = apiBehaviorOptions;
        _db = db;
    }
    
    [HttpPost]
    [Route("login/id")]
    public async Task<ActionResult<JwtData>> Login([FromBody] LoginViaIdModel model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);

        if (user == null)
        {
            ModelState.AddModelError("UserId", "User not found");
            return (ActionResult)_apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
            
        var token = await GetToken(user);
    
        return Ok(new JwtData()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<User>> CurrentUserInfo()
    {
        var user = HttpContext.GetUser();
        return user;
    }

    private async Task<JwtSecurityToken> GetToken(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        return GetToken(authClaims);
    }

    public static JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            expires: DateTime.Now.AddDays(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}