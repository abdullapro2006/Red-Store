using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedStore.Database;
using RedStore.Services.Abstract;
using RedStore.ViewModels.Product;

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
                _userService.CurrentUser
            );

        _redStoreDbContext.SaveChanges();

        return RedirectToAction("index", "home");
    }


    [HttpGet("remove-basket-product")]
    public IActionResult RemoveBasketProduct(int basketProductId)
    {
        var basketProduct = _redStoreDbContext
            .BasketProducts
            .FirstOrDefault(bp => bp.UserId == _userService.CurrentUser.Id && bp.Id == basketProductId);

        if(basketProduct == null)
        {
            return NotFound();
        }

        _redStoreDbContext.BasketProducts.Remove(basketProduct);


        _redStoreDbContext.SaveChanges();

        return RedirectToAction("cart", "basket");
    }

    [HttpGet("increase-basket-product")]
    public IActionResult IncreaseBasketProduct(int basketProductId)
    {
        var basketProduct = _redStoreDbContext
            .BasketProducts
            .FirstOrDefault(bp => bp.UserId == _userService.CurrentUser.Id && bp.Id == basketProductId);

        if (basketProduct == null)
        {
            return NotFound();
        }

        basketProduct.Quantity++;


        _redStoreDbContext.SaveChanges();

        return Json(new BasketQuantityUpdateResponseViewModel
        {
            Total = basketProduct.Quantity * basketProduct.Product.Price,
            Quantity = basketProduct.Quantity
        });


    }


    [HttpGet("decrease-basket-product")]
    public IActionResult DecreaseBasketProduct(int basketProductId)
    {
        var basketProduct = _redStoreDbContext
            .BasketProducts
            .FirstOrDefault(bp => bp.UserId == _userService.CurrentUser.Id && bp.Id == basketProductId);

        if (basketProduct == null)
        {
            return NotFound();
        }

        basketProduct.Quantity--;

        if (basketProduct.Quantity <= 0)
        {
            _redStoreDbContext.BasketProducts.Remove(basketProduct);
            _redStoreDbContext.SaveChanges();

            return NoContent();
        }



           else
           {
            _redStoreDbContext.SaveChanges();

            var updateResponseViewModel = new BasketQuantityUpdateResponseViewModel
            {
                Total = basketProduct.Quantity * basketProduct.Product.Price,
                Quantity = basketProduct.Quantity
            };

             return Json(updateResponseViewModel);
           }
    }


    [HttpGet("cart")]
    public IActionResult GetBasketProducts()
    {
        var basketProducts = _redStoreDbContext.BasketProducts
            .Where(bp => bp.UserId == _userService.CurrentUser.Id)
            .Include(bp => bp.Product)
            .Include(bp => bp.Color)
            .Include(bp => bp.Size)
            .ToList();

        return View(basketProducts);
    }
}
