using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.ViewModels;
using RedStore.ViewModels.Product;
using System.Reflection;

namespace RedStore.Contollers.Admin;
[Route("admin/products")]
public class ProductController : Controller
{
    private readonly RedStoreDbContext _redStoreDbContext;
    private readonly ILogger<ProductController> _logger;
    public ProductController(RedStoreDbContext redStoreDbContext,
        ILogger<ProductController> logger)
    {
        _redStoreDbContext = redStoreDbContext;

        _logger = logger;
    }


    #region Products
    [HttpGet]
    public IActionResult Products()
    {
        var products = _redStoreDbContext.Products
            .Include(p => p.Category)
            .ToList();

        return View("Views/Admin/Product/Products.cshtml",products);
    }

    #endregion


    #region Add

    [HttpGet("add")]
    public IActionResult Add()
    {
        var model = new ProductAddResponseViewModel
        {
            Categories = _redStoreDbContext.Categories.ToList()
        };

        return View("Views/Admin/Product/ProductAdd.cshtml", model);
    }


 
    [HttpPost("add")]
    public IActionResult Add(ProductAddRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
        }


        if(model.CategoryId != null)
        {
            var category = _redStoreDbContext.Categories.FirstOrDefault(c => c.Id == model.CategoryId.Value);

            if(category == null)
            {
                ModelState.AddModelError("CategoryId", "Category doesn't exist");

                return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
            }
        }

        var product = new Product
        {
            Name = model.Name,
            Price = model.Price,
            Rating = model.Rating,
            CategoryId = model.CategoryId,
        };

        try
        {
            _redStoreDbContext.Products.Add(product);
            _redStoreDbContext.SaveChanges();
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Postgresql Exception");

            throw e;
            
        }

      


        return RedirectToAction("Products");
    }

    #endregion


    #region Edit
    [HttpGet("edit")]
    public IActionResult Edit(int id)
    {
    
        Product product = _redStoreDbContext.Products.FirstOrDefault(p => p.Id == id);  

        if (product == null)
        {
            return NotFound();
        }


        var model = new ProductUpdateResponseViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Rating = product.Rating,
            Categories = _redStoreDbContext.Categories.ToList(),
            CategoryId = product.CategoryId,
        };

        return View("Views/Admin/Product/ProductEdit.cshtml", model);
    }


    [HttpPost("edit")]
    public IActionResult Edit(ProductUpdateRequestViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return PrepareValidationView("Views/Admin/Product/ProductEdit.cshtml");
        }

        if (model.CategoryId != null)
        {
            var category = _redStoreDbContext.Categories.FirstOrDefault(c => c.Id == model.CategoryId.Value);

            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Category doesn't exist");

                return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
            }
        }



        Product product = _redStoreDbContext.Products.FirstOrDefault(p => p.Id == model.Id);

        if (product == null)
        {
            return NotFound();
        }



        product.Name = model.Name;
        product.Price = model.Price;
        product.Rating = model.Rating;
        product.CategoryId = model.CategoryId;


        try
        {

            _redStoreDbContext.Products.Update(product);
            _redStoreDbContext.SaveChanges();
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Postgresql Exception");

            throw e;

        }

        return RedirectToAction("Products");
    }

    #endregion


    private ViewResult PrepareValidationView(string viewName)
    {
      

            var responseViewModel = new ProductAddResponseViewModel
            {
                Categories = _redStoreDbContext.Categories.ToList()
            };

            return View(viewName, responseViewModel);
        
    }

    #region Delete
    [HttpGet("delete")]
    public IActionResult Delete(int id)
    {
        Product product = _redStoreDbContext.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }
        _redStoreDbContext.Remove(product);
        _redStoreDbContext.SaveChanges();


        return RedirectToAction("Products");
    }

    #endregion



}

