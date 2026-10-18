using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PasswordVaultService.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["UserName"] = User.Identity?.Name;
            ViewData["UserEmail"] = User.Identity?.IsAuthenticated == true ? User.FindFirst(ClaimTypes.Email)?.Value : "N/A";

            return View();
        }
    }
}