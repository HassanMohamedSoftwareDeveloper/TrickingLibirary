using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("logout")]
    public async Task<IActionResult> Logout(string logoutId, [FromServices] SignInManager<IdentityUser> signInManager,
        [FromServices] IIdentityServerInteractionService interactionService)
    {
        await signInManager.SignOutAsync();
        var logoutContext = await interactionService.GetLogoutContextAsync(logoutId);
        if (string.IsNullOrWhiteSpace(logoutContext.PostLogoutRedirectUri)) return BadRequest();
        return Redirect(logoutContext.PostLogoutRedirectUri);
    }
}
