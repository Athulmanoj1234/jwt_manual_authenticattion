in permission based authorization system the system does not needs to store every permission from the system when request comes the user get identifies and checks the permission from the db.


high level digram

Client sends GET request
GET /Get-User/ + Bearer JWT
          |
          |
          |
JWT Bearer authentication
Validates signature, issuer, audience and expiration
          |
          |
          |
HasPermissionAttribute
Converts CanInspectUser into a policy name
          |
          |
          |
PermissionAuthorizationPolicyProvider
Creates a policy containing PermissionRequirement
          |
          |
          |
PermissionAuthorizationHandler
Reads the user ID from the authenticated claims
          |
          |
          |
IPermissionService → Entity Framework Core
Retrieves the user's roles and their permissions
          |
          |
          |
Authorization decision
Permission exists → action runs; otherwise access is denied


user to role -> many to many relationship
role to permissions -> many to many relationship  
  -> so one user will have many roles and that role will assigned to many users.
  -> one role will have many permissions and that one permission will be user by many roles.


Suppose your database contains the following data after your migrations and a user has been assigned the registered role.
Roles table
Value	Name
1	registered
Permissions table
Id	Name
1	CanInspectUser
2	CanUpdateUser
RolePermission table
RoleId	PermissionId
1	1
1	2


code of the class RolePermissionConfiguration 

        public void Configure(EntityTypeBuilder<RolePermission> builder) {

            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            // <summary>
            //now for each roles a corresponding permission needed to be added so add a heper method to create role
            //</summary>
            builder.HasData(
                Create(Role.registered, Permissions.CanInspectUser),
                Create(Role.registered, Permissions.CanUpdateUser)
                );
                
        }
        public static RolePermission Create(Role role, Permissions permission) {
            return new RolePermission
            {
                RoleId = role.Value,
                PermissionId = (int)permission,
            };
        }

so when the data base configuration ie in this rolepermissionconfiguration is done you can see that initially to the rolepermission many-to-many table for the role registered both permissions "caninspect" and "canupdate" the user is done.

first when the request is received in the backend - (GET /ape/users/Get-User/2) with Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
 ie the user A(id-1) request for the user b(id-2) data 

so first app.UseAuthentication() runs - 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;

    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:key"]))
        };
});

->ASP.NET Core selects the JWT Bearer handler as its default authentication handler.
->The handler extracts the bearer token from the Authorization header.
->It validates the signature using IssuerSigningKey, checks the issuer and audience, and verifies the token's lifetime.
->If validation succeeds, it creates an authenticated ClaimsPrincipal containing the claims from the token.
so the result principle will be - 
ClaimsPrincipal
    └── ClaimsIdentity (authenticated)
          └── sub = 11111111-1111-1111-1111-111111111111

Important: SaveToken = true is not what makes the token available to the authentication handler. It allows the successfully authenticated token to be stored in the authentication properties so it can be retrieved with GetTokenAsync("access_token") in applicable authentication flows.

->so check if user sent any jwt(access) tokens if user sent the access tokens then it will get the user data from the token 

5. ASP.NET Core discovers your HasPermission attribute
Your controller contains:
[HasPermission(Permissions.CanInspectUser)]
[HttpGet("Get-User/{id}")]
public async Task<IActionResult> GetUser(Guid id)

so the HasPermission class is - 
public sealed class HasPermissionAttribute
    : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permission)
        : base(policy: permission.ToString())
    {
    }
}

so CanInspectUser is converted into string and send to the base constructor

-> and then the app.UseAuthorization() runs but provide anomious access because now the endpoint has the [Haspermission] Atteribute


6. the custom policy provider creates the policy - 
public class PermissionAuthorizationPolicyProvider
    : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?>
        GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy =
            await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        return new AuthorizationPolicyBuilder()
            .AddRequirements(
                new PermissionRequirement(policyName))
            .Build();
    }
}

 -> first it asks for the policy - GetPolicyAsync("CanInspectUser") -> ie it looks for the existing policy - 
AuthorizationPolicy? policy =
    await base.GetPolicyAsync(policyName);

if the policy is not null we return the policy 
            if (policy is not null)
            {
                return policy;
            }
otherwise we creates the policy the came name and we return it
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

it is by requirement object is passed to Authorization

8. Your PermissionAuthorizationHandler starts executing
Here is the core of your handler:
Let's go line by line.
Step 8.1 — Read the authenticated user's claims
context.User is the ClaimsPrincipal created by authentication. The authorization handler doesn't decode the JWT again. It uses the identity and claims already established by the authentication process.
Then:

Now we return to this code in your handler:
At this point, the handler has two important values.
The database returned:
The policy provider created:
The handler executes:
This is equivalent to:
Since the permission exists, the condition succeeds.
Then:
This tells ASP.NET Core that this specific authorization requirement has been satisfied.

-> then takes the userId from the claims and based on the userid fetched the permissions for that role for that user from the db
-> if that permission contains the the authorize attibutes policy permission the the request succeeds 
if (permissions != null && permissions.Count != 0 && permissions.Contains(requirement._permission)) {
     context.Succeed(requirement); 
}



