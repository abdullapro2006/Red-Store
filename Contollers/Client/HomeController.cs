using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedStore.Database;

namespace RedStore.Contollers.Client;

public class HomeController : Controller
{
    private readonly RedStoreDbContext _dbContext;

    public HomeController(RedStoreDbContext redStoreDbContext)
    {
        _dbContext = redStoreDbContext;
    }

    public ViewResult Index()
    {
        return View(_dbContext.Products.ToList());
    }
    public ViewResult About()
    {
        return View();
    }

}
