using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;

namespace FruitSA.Web;

public abstract class SecurePageModel : PageModel
{
    protected string Token { get; private set; }
    protected string Role { get; private set; }
    protected virtual string RequiredRole => null;

    public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
    {
        Token = HttpContext.Session.GetString("JWToken");
        if (string.IsNullOrEmpty(Token))
        {
            context.Result = new RedirectToPageResult("/Account/Login");
            return;
        }
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.ReadJwtToken(Token);
        Role = jwt.Claims.FirstOrDefault(c => c.Type == "role" || c.Type.EndsWith("role"))?.Value;

        // Enforce role if set
        if (RequiredRole != null && Role != RequiredRole)
        {
            context.Result = new ForbidResult();
        }
        base.OnPageHandlerExecuting(context);
    }


}
