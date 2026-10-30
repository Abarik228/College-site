using System.Security.Claims;
using CollegePortal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegePortal.Controllers;

public class AccessController(CollegeStore store) : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "College");
        }

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string login, string password, string? returnUrl = null)
    {
        var account = store.FindAccount(login);
        if (account is null || !account.Login(login, password))
        {
            ViewBag.Error = "Неверный логин или пароль.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, account.LoginName),
            new Claim(ClaimTypes.Role, account.RoleUser.RoleName),
            new Claim("DisplayName", account.RoleUser.VisibleName)
        };
        var identity = new ClaimsIdentity(claims, "CollegePortal.Cookie");
        await HttpContext.SignInAsync("CollegePortal.Cookie", new ClaimsPrincipal(identity));

        return Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl!)
            : RedirectToAction("Index", "College");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var account = store.FindAccount(User.Identity?.Name ?? string.Empty);
        account?.Logout();
        await HttpContext.SignOutAsync("CollegePortal.Cookie");
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public IActionResult Denied() => View();
}