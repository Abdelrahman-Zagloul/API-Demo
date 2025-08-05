using API_Demo.Attributes;

namespace API_Demo.Model
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public Permission Permissions { get; set; }
    }
}
