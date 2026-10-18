using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PasswordVaultService.Business.Interfaces.Services;
using PasswordVaultService.Models;
using System.Security.Claims;

namespace PasswordVaultService.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.RegisterUserAsync(model.Name, model.Email, model.Password);
                    // Sucesso no registro, redirecionar para uma página de confirmação ou login
                    TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Verifique seu e-mail para confirmação.";
                    return RedirectToAction("Login", "Account"); // Ou uma página de "Confirme seu E-mail"
                }
                catch (System.InvalidOperationException ex) // Exceção para e-mail já em uso
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (System.Exception)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar registrar. Tente novamente.");
                }
            }

            return View(model);
        }

        // GET: Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);

                if (user != null)
                {
                    // Se o e-mail não for confirmado, você pode bloquear o login aqui:
                    // if (!user.IsEmailConfirmed)
                    // {
                    //     ModelState.AddModelError(string.Empty, "Sua conta ainda não foi confirmada. Verifique seu e-mail.");
                    //     return View(model);
                    // }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas. Verifique seu e-mail e senha.");
                }
            }
            return View(model);
        }

        // GET: Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}