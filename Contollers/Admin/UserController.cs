using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedStore.Database;
using RedStore.ViewModels.Auth;

namespace RedStore.Contollers.Admin;

[Route("admin/users")]
[Authorize(Roles = "admin")]
public class UserController : Controller
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public UserController(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }

    #region Users

    [HttpGet]
    public IActionResult Users()
    {
        var users = _redStoreDbContext.Users
            .OrderBy(u => u.Name)
            .ToList();

        return View("Views/Admin/User/Users.cshtml", users);
    }

    #endregion

    #region Edit user

    [HttpGet("{id}/edit")]
    public IActionResult Edit(int id)
    {
        var user = _redStoreDbContext.Users
            .FirstOrDefault(o => o.Id == id);

        if (user == null)
            return NotFound();

        var model = new UserUpdateViewModel
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            IsAdmin = user.IsAdmin,
            Email = user.Email
        };

        return View("Views/Admin/User/UserEdit.cshtml", model);
    }

    [HttpPost("{id}/edit")]
    public IActionResult Edit([FromRoute] int id, [FromForm] UserUpdateViewModel model)
    {
        var user = _redStoreDbContext.Users
            .FirstOrDefault(o => o.Id == id);

        if (user == null)
            return NotFound();

        user.Name = model.Name;
        user.LastName = model.LastName;
        user.IsAdmin = model.IsAdmin;

        _redStoreDbContext.SaveChanges();

        return RedirectToAction("Index");
    }

    #endregion
}
