using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RedStore.Contracts;
using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Services.Abstract;
using RedStore.Services.Concretes;
using RedStore.ViewModels;
using RedStore.ViewModels.Product;
using System.Reflection;

namespace RedStore.Contollers.Admin;
[Route("admin/products")]
[Authorize(Roles = "admin")]
public class ProductController : Controller
{
    private readonly RedStoreDbContext _redStoreDbContext;
    private readonly ILogger<ProductController> _logger;
    private readonly IFileService _fileService;
    public ProductController(RedStoreDbContext redStoreDbContext,
        ILogger<ProductController> logger,
        IFileService fileService)
    {
        _redStoreDbContext = redStoreDbContext;

        _logger = logger;
        _fileService = fileService;
    }


    #region Products
    [HttpGet(Name = "admin-products")]
    public IActionResult Products()
    {
        var products = _redStoreDbContext.Products
            .Include(p => p.Category)
            .Include(p => p.ProductColors)
            .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductSizes)
            .ThenInclude(ps  => ps.Size)
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
            Categories = _redStoreDbContext.Categories.ToList(),
            Colors = _redStoreDbContext.Colors.ToList(),
            Sizes = _redStoreDbContext.Sizes.ToList(),
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

        try
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Rating = model.Rating,
                CategoryId = model.CategoryId,
            };
            _redStoreDbContext.Products.Add(product);

            foreach (var colorId in model.SelectedColorIds)
            {
                var color = _redStoreDbContext.Colors.FirstOrDefault(c => c.Id == colorId);

                if (color == null)
                {
                    ModelState.AddModelError("SelectedColorIds", "Color doesn't exist");

                    return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
                }

                var productColor = new ProductColor
                {
                    ColorId = colorId,
                    Product = product
                };


                _redStoreDbContext.ProductColors.Add(productColor);
            }

            foreach (var sizeId in model.SelectedSizeIds)
            {
                var size = _redStoreDbContext.Colors.FirstOrDefault(c => c.Id == sizeId);

                if (size == null)
                {
                    ModelState.AddModelError("SelectedSizeIds", "Size doesn't exist");

                    return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
                }

                var productSize = new ProductSize
                {
                    SizeId = sizeId,
                    Product = product
                };


                _redStoreDbContext.ProductSizes.Add(productSize);
            }

            string uniqueFileName = _fileService.Upload(model.Image, UploadDirectory.Products);
            product.ImageName = model.Image.FileName;
            product.ImageNameInFileSytem = uniqueFileName;

            _redStoreDbContext.SaveChanges();

           
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "PostgreSql Exception");
        }
        




        return RedirectToAction("Products");
    }

    #endregion


    #region Edit
    [HttpGet("edit")]
    public IActionResult Edit(int id)
    {
    
        Product product = _redStoreDbContext.Products
            .Include(p => p.ProductColors)
            .Include(p => p.ProductSizes)
            .FirstOrDefault(p => p.Id == id);  

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
            SelectedColorIds = product.ProductColors.Select(pc => pc.ColorId).ToArray(),
            Colors = _redStoreDbContext.Colors.ToList(),
            SelectedSizeIds = product.ProductSizes.Select(pc => pc.SizeId).ToArray(),
            Sizes = _redStoreDbContext.Sizes.ToList(),
            ImageNameInFileSystem = product.ImageNameInFileSytem
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



        Product product = _redStoreDbContext.Products
            .Include(pc => pc.ProductColors)
            .Include(ps => ps.ProductSizes)
            .FirstOrDefault(p => p.Id == model.Id);

        if (product == null)
        {
            return NotFound();
        }

        #region ProductColor
        var productColorIdsInDb = product.ProductColors.Select(pc => pc.ColorId);



        var removableIds = productColorIdsInDb
            .Where(id => !model.SelectedColorIds.Contains(id))
            .ToList();

        product.ProductColors.RemoveAll(pc => removableIds.Contains(pc.ColorId));

        var addableIds = model.SelectedColorIds
            .Where(id => !productColorIdsInDb.Contains(id))
            .ToList();


        var newProductColors = addableIds.Select(colorId => new ProductColor
        {
            ColorId = colorId,
            Product = product
        });

        product.ProductColors.AddRange(newProductColors);

        #endregion

        #region ProductSize
        var productSizeIdsInDb = product.ProductSizes.Select(pc => pc.SizeId);



        var removableSizeIds = productSizeIdsInDb
            .Where(id => !model.SelectedSizeIds.Contains(id))
            .ToList();

        product.ProductSizes.RemoveAll(pc => removableSizeIds.Contains(pc.SizeId));

        var addableSizeIds = model.SelectedSizeIds
            .Where(id => !productSizeIdsInDb.Contains(id))
            .ToList();


        var newProductSizes = addableSizeIds.Select(sizeId => new ProductSize
        {
            SizeId = sizeId,
            Product = product
        });

        product.ProductSizes.AddRange(newProductSizes);

        #endregion

        #region Image

        if(model.Image != null)
        {
            _fileService.Delete(UploadDirectory.Products, product.ImageNameInFileSytem);


            product.ImageName = model.Image.FileName;
            product.ImageNameInFileSytem = _fileService.Upload(model.Image, UploadDirectory.Products);
        }

       

        #endregion


        try
        {

            product.Name = model.Name;
            product.Price = model.Price;
            product.Rating = model.Rating;
            product.CategoryId = model.CategoryId;

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


   

    #region Delete
    [HttpGet("delete")]
    public IActionResult Delete(int id)
    {
        Product product = _redStoreDbContext.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }
        _redStoreDbContext.Remove(product);
        _redStoreDbContext.SaveChanges();

        _fileService
            .Delete(UploadDirectory.Products, product.ImageNameInFileSytem);

        return RedirectToAction("Products");
    }

    #endregion

    private ViewResult PrepareValidationView(string viewName)
    {


        var responseViewModel = new ProductAddResponseViewModel
        {
            Categories = _redStoreDbContext.Categories.ToList(),
            Colors = _redStoreDbContext.Colors.ToList(),
            Sizes = _redStoreDbContext.Sizes.ToList(),
        };

        return View(viewName, responseViewModel);

    }

}

