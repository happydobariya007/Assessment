using System.Security.Claims;
using AssessmentProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AssessmentProject.Service.Attributes;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var jwtService = context.HttpContext.RequestServices.GetService(typeof(IJwtService)) as IJwtService;
        var token = jwtService.GetJWTToken(context.HttpContext.Request);
        var principal = jwtService?.ValidateToken(token ?? "");
        if (principal == null)
        {
            context.Result = new RedirectToActionResult("Login", "User", null);
            return;
        }

        context.HttpContext.User = principal;

        if (_roles.Length > 0)
        {
            var userRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (!int.TryParse(userRole?.Value, out int roleId))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }
            string roleName = roleId switch
            {
                1 => "Admin",
                2 => "User",
                _ => "Unknown"
            };

            if (!_roles.Contains(roleName))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }
        }
    }
}
