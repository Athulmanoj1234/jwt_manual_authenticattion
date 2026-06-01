using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace jwtmanualauthentication.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder) {

            builder.ToTable("Roles");

            builder.HasKey(r => r.Value); // here value means id

            builder.HasMany(r => r.Users)
                .WithMany();


            //
            //<summary>
            //   here we have permissions and role can have
            //   many permissions and permission can have many roles
            //   and the joint table is joined by usingentity<manyto many enitty>()
            //</summary>
            builder.HasMany(r => r.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();
            

            //now we need to seed the data into the roles table ie initial data needs to be inserted - so we use builder.hasData()
            builder.HasData(Role.Enumerations.Values.ToList()); // we take only values from the dictionary and return it as list.
        }
    }
}
