using jwtmanualauthentication.Common;
using System.Collections.ObjectModel;

namespace jwtmanualauthentication.Models.Enities
{
    public class Role : Enumeration<Role>
    {
        public static readonly Role registered = new(1, "registered");

        public Role(int Value, string Name) : base(Value, Name)
        {
            
        }

        public ICollection<Permission> Permissions { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
