namespace API_Demo.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttribute : Attribute
    {

        public Permission Permission { get; }
        public CheckPermissionAttribute(Permission permission)
        {
            Permission = permission;
        }
    }
    public enum Permission
    {
        Read = 1,
        Create,
        Update,
        Delete,
    }
}
