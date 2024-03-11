using Microsoft.AspNetCore.Mvc;

namespace RedStore.Contollers.Client;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SingleProduct(int id)
    {
        return View();
    }
}
