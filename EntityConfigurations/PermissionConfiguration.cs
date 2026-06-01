using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jwtmanualauthentication.EntityConfigurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder) {

            builder.ToTable("permissions");

            builder.HasKey(p => p.Id);

            // now convert the enums - permissions values into enity - permissions
            IEnumerable<Permission> permissionValues = Enum.GetValues<Permissions>()
                .Select(p => new Permission() {
                    Id = (int)p,
                    Name = p.ToString()
                });

            builder.HasData(permissionValues);
        }
    }
}
