using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;

namespace jwtmanualauthentication.AuthorizationHandler
{
    //public class PermissionService : IPermissionService
    public class PermissionService
    {
        public ApplicationDbContext _context;
        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<HashSet<string>> GetPermissionsAsync(Guid memberId)
        //{
        //    ICollection<Role>[] roles = await _context.Set<User>()
        //        .Include(x => x.Roles)
        //        .ThenInclude(x => x.Permissions)
        //        .Where(x => x.Id == memberId)
        //        .Select(x => x.Roles)
        //        .ToArrayAsync();

        //    return roles
        //        .SelectMany(x => x)
        //        .SelectMany(x => x.Permissions)
        //        .Select(x => x.Name)
        //        .ToHashSet();
        //}
    }
}
