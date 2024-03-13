using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedStore.Database;
using RedStore.Services.Abstract;

namespace RedStore.Contollers.Client;

[Route("basket")]
public class BasketController : Controller
{
    private readonly IUserService _userService;
    private readonly RedStoreDbContext _redStoreDbContext;
    private readonly IProductService _productService;
    private readonly IBasketService _basketService;

    public BasketController(
        IUserService userService,
        RedStoreDbContext redStoreDbContext,
        IProductService productService,
        IBasketService basketService)
    {
        _userService = userService;
        _redStoreDbContext = redStoreDbContext;
        _productService = productService;
        _basketService = basketService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("add-product")]
    public IActionResult AddProduct(int productId, int? sizeId, int? colorId)
    {
        _basketService.CreateOrIncrementQuantity
            (
                productId,
                sizeId ?? _productService.GetDefaultSizeId(productId),
                colorId ?? _productService.GetDefaultColorId(productId),
                _userService.GetCurrentLoggedUser()
            );

        _redStoreDbContext.SaveChanges();

        return RedirectToAction("index", "home");
    }


    [HttpGet("cart")]
    public IActionResult GetBasketProducts()
    {
        var basketProducts = _redStoreDbContext.BasketProducts
            .Where(bp => bp.UserId == _userService.GetCurrentLoggedUser().Id)
            .Include(bp => bp.Product)
            .Include(bp => bp.Color)
            .Include(bp => bp.Size)
            .ToList();

        return View(basketProducts);
    }
}
