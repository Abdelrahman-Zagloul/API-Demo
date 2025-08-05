using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using API_Demo.Attributes;
using API_Demo.Data;

namespace API_Demo.Filter
{
    public class PermissionBeasdOnAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly AppDbContext? _context;

        public PermissionBeasdOnAuthorization(AppDbContext? context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //get attribute 
            var attribute = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is CheckPermissionAttribute) as CheckPermissionAttribute;
            if (attribute != null)
            {
                var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    context.Result = new ForbidResult();
                bool hasPermission = _context.Permissions.Any(x => x.ApplicationUserId == userId && x.Permissions == attribute.Permission);

                if (!hasPermission)
                    context.Result = new ForbidResult();
            }
            else
                context.Result = new ForbidResult();
        }
    }
}
