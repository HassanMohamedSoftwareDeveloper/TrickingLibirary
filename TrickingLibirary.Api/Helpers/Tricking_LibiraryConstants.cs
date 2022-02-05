using IdentityServer4;

namespace TrickingLibirary.Api.Helpers;

public static class Tricking_LibiraryConstants
{
    public struct Policies
    {
        public const string User = IdentityServerConstants.LocalApi.PolicyName;
        public const string Mod = nameof(Mod);
    }
    public struct IdentityResources
    {
        public const string RoleScope = "role";
    }
    public struct Claims
    {
        public const string Role = "role";
    }
    public struct Roles
    {
        public const string Mod = nameof(Mod);
    }
}
