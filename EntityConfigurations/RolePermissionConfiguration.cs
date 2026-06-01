using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace jwtmanualauthentication.EntityConfigurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
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
    }
}
