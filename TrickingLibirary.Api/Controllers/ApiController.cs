using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibirary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    #region PROPS :
    protected string UserId => GetClaim(JwtClaimTypes.Subject);
    protected string Username => GetClaim(JwtClaimTypes.PreferredUserName);
    #endregion
    #region Helpers :
    private string GetClaim(string claimType) =>
        User.Claims.FirstOrDefault(x => x.Type.Equals(claimType))?.Value;
    #endregion
}
