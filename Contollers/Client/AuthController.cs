using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RedStore.Database;
using RedStore.ViewModels.Auth;
using System.Security.Claims;

namespace RedStore.Contollers.Client;

public class AuthController : Controller
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public AuthController(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = _redStoreDbContext.Users
            .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

        if (user == null)
        {
            ModelState.AddModelError("Email", "Password or email is wrong");
            return View();
        }


        var claims = new List<Claim>(){
          new Claim("Id", user.Id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

       await HttpContext.SignInAsync("Cookies", claimsPrincipal);


        return RedirectToAction("index","home");
    }

      public async Task<IActionResult> Logout()
      {
        await HttpContext.SignOutAsync("Cookies");

        return RedirectToAction(nameof(Login));
      }
}
