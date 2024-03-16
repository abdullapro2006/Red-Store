using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RedStore.Database;
using RedStore.Database.DomainModels;
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

    #region Login

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

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "admin"));
        }

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

       await HttpContext.SignInAsync("Cookies", claimsPrincipal);


        return RedirectToAction("index","home");
    }



    #endregion

    #region Register

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        if(_redStoreDbContext.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "This email already taken");
            return View();
        }


        var user = new User
        {
            Name = model.Email,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password
        };

        _redStoreDbContext.Users.Add(user);
        _redStoreDbContext.SaveChanges();
        return RedirectToAction("index", "home");
    }



    #endregion

    #region Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");

        return RedirectToAction(nameof(Login));
    }
    #endregion
}
