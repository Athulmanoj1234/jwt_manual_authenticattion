for jwt installing jwt :
    System.IdentityModel.Tokens.Jwt
    Microsoft.AspNetCore.Authentication.JwtBearer


sample entity db columns

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//DataAnnotations provides attributes like [Key], [Required], etc.

Schema provides attributes like [Table] and [Column] to define how your class maps to a database table.

namespace DemoWebApi.Entities
This organizes the code into a logical group.
Here, it indicates that this class belongs to the Entities folder of the DemoWebApi project.
{
This maps the User class to the "user" table in the database.
If you don’t specify this, EF Core will by default create a table named Users (plural form of the class).
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

[Key] → Marks this property as the primary key of the table.[DatabaseGenerated(DatabaseGeneratedOption.Identity)] → Tells EF Core that the database will automatically generate this value (e.g., auto-increment ID).[Column("id")] → Maps this property to the "id" column in the table.

        [Column("user_name")]
        public string? Username { get; set; }
[Column("user_name")] → Maps the Username property to the user_name column in the table.
The ? means the field is nullable (it can store null values).


        [Column("password_hash")]
        public string? PasswordHash { get; set; }
    
   