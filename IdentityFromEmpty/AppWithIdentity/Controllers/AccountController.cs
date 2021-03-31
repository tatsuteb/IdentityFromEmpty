using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppWithIdentity.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AppWithIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly Dictionary<string, string> _store;

        public AccountController(Dictionary<string, string> store)
        {
            _store = store;
        }

        #region Register

        public IActionResult Register([FromQuery] string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterModel model, [FromQuery]string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            returnUrl = string.IsNullOrWhiteSpace(returnUrl)
                ? Url.Action(action: "Index", controller: "Home")
                : returnUrl;

            if (!_store.TryAdd(model.Email, model.Password))
            {
                model.Password = "";

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email),
            };
            
            var claimsIdentity = new ClaimsIdentity(
                claims, 
                CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                RedirectUri = returnUrl
            };
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);

            return LocalRedirect(returnUrl);
        }

        #endregion

        #region LogIn
        
        public IActionResult Login([FromQuery]string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginModel model, [FromQuery]string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            returnUrl = string.IsNullOrWhiteSpace(returnUrl)
                ? Url.Action(action: "Index", controller: "Home")
                : returnUrl;

            if (!_store.TryGetValue(model.Email, out var password))
            {
                model.Password = "";

                return View(model);
            }

            if (model.Password != password)
            {
                model.Password = "";

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email),
            };
            
            var claimsIdentity = new ClaimsIdentity(
                claims, 
                CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                RedirectUri = returnUrl
            };
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);

            return LocalRedirect(returnUrl);
        }

        #endregion


        #region LogOut

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
