using Microsoft.AspNetCore.Authorization;

namespace jwtmanualauthentication.AuthorizationHandler
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string Permission)
        {
            this._permission = Permission;
        }
        public string _permission { get; set; }

    }
}
