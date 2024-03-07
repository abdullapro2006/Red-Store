using Microsoft.AspNetCore.Mvc;
using RedStore.Database;

namespace RedStore.Contollers.Admin;

[Route("admin/colors")]
public class ColorController : Controller
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public ColorController(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }

    [HttpGet]
    public IActionResult Colors()
    {
        var colors = _redStoreDbContext.Colors.ToList();

        return View("Views/Admin/Color/Colors.cshtml",colors);
    }
}
