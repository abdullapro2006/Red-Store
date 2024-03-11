using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedStore.Database;

namespace RedStore.Contollers.Client;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SingleProduct(int id, [FromServices] RedStoreDbContext redStoreDbContext)
    {
        var product = redStoreDbContext.Products
            .Include(p => p.ProductColors)
            .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductSizes)
            .ThenInclude(ps => ps.Size)
            .FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
}
