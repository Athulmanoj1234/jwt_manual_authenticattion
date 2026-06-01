using Microsoft.AspNetCore.Authorization;

namespace jwtmanualauthentication.Atttributes
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permissions permission) : base(policy: permission.ToString())  
        {
            // in this primary constructor the permission will go to the [Authorize(permission ie CanInspectUser)] attribute internally and check the policy that the request is authorized       
        }
    }
}


public enum Permissions {
    CanInspectUser = 1,
    CanUpdateUser = 2,
}
