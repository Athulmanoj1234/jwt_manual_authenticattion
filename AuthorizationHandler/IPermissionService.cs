namespace jwtmanualauthentication.AuthorizationHandler
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(Guid memberId);
    }
}
