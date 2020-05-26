using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShineSpike.Services;
using ShineSpike.Utils;
using ShineSpike.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShineSpike.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly ILoginService Service;

        public AccountController(ILoginService service) => Service = service;

        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData[Constants.ReturnUrl] = returnUrl;
            return View();
        }

        [Route("login")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl, LoginViewModel model)
        {
            ViewData[Constants.ReturnUrl] = returnUrl;

            if (model?.IsValid() == false || !ModelState.IsValid || !Service.ValidateLogin(model))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(nameof(Login), model);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));

            await HttpContext.SignInAsync(
                principal: new ClaimsPrincipal(identity), 
                properties: new AuthenticationProperties { IsPersistent = model.RememberMe }
            );

            return LocalRedirect(returnUrl ?? "/");
        }

        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}
