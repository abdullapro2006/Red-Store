using Microsoft.AspNetCore.Mvc;
using RedStore.Database;
using RedStore.Database.Repositories;

namespace RedStore.Contollers.Client;

public class HomeController : Controller
{
    private readonly ProductRepository _productRepository;

    public HomeController()
    {
        _productRepository = new ProductRepository();
    }

    public ViewResult Index()
    {
        return View(_productRepository.GetAll());
    }
    public ViewResult About()
    {
        return View();
    }

    protected override void Dispose(bool disposing)
    {
        _productRepository.Dispose();

        base.Dispose(disposing);
    }
}
