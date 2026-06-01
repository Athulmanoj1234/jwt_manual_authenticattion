using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace jwtmanualauthentication.AuthorizationHandler
{
    //<summary>for this authorization handler we should contain a authorizaiton requirment ie in here is permission requirement</summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>  
    {
        public IServiceScopeFactory _serviceScopeFactory { get; set; } // we are adding new service scope factory inside the permission authorization handler because we cannot register a scoped service inside a single servicce

        public PermissionAuthorizationHandler(IServiceScopeFactory ServiceScopeFactory)
        {
            this._serviceScopeFactory = ServiceScopeFactory;
        }

        //<summary>used to overide the existing handleREquirement jwt method first access the member id from the claims subject</summary>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,  // this is the context that is recived from the user token and in this part the token info like claims is contained
            PermissionRequirement requirement // the requirement that is in AuthorizationHandler
            ) {
            string? memberId = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            if (!Guid.TryParse(memberId, out Guid parsedMemberId)) {
                return;
            }

            var scope = this._serviceScopeFactory.CreateScope();
            
            //so next we create the service using the scope
            IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            // so from this permission service there will be hashset of the type string permissions is returned
            var permissions = await permissionService.GetPermissionsAsync(parsedMemberId);
            if (permissions != null && permissions.Count != 0 && permissions.Contains(requirement._permission)) {
                 context.Succeed(requirement); 
            }
        }
    }
}
