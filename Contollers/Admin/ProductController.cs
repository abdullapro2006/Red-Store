﻿using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Database.Repositories;
using RedStore.ViewModels;
using System.Reflection;

namespace RedStore.Contollers.Admin;
[Route("admin/products")]
public class ProductController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly CategoryRepository _categoryRepository;
    public ProductController()
    {
        _productRepository = new ProductRepository();
        _categoryRepository = new CategoryRepository();
    }


    #region Products
    [HttpGet]
    public IActionResult Products()
    {
        return View("Views/Admin/Product/Products.cshtml",_productRepository.GetAllWithCategories());
    }

    #endregion


    #region Add

    [HttpGet("add")]
    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAll();
        var model = new ProductAddResponseViewModel
        {
            Categories = categories
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

        var product = new Product
        {
            Name = model.Name,
            Price = model.Price,
            Rating = model.Rating,
            CategoryId = model.CategoryId,
        };

        _productRepository.Insert(product);


        return RedirectToAction("Products");
    }

    #endregion


    #region Edit
    [HttpGet("edit")]
    public IActionResult Edit(int id)
    {
        if (!ModelState.IsValid)
        {
            return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
        }

        Product product = _productRepository.GetById(id);

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
            Categories = _categoryRepository.GetAll(),
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


        Product product = _productRepository.GetById(model.Id);

        if (product == null)
        {
            return NotFound();
        }
        product.Name = model.Name;
        product.Price = model.Price;
        product.Rating = model.Rating;
        product.CategoryId = model.CategoryId;


        _productRepository.Update(model);

        return RedirectToAction("Products");
    }

    #endregion


    private ViewResult PrepareValidationView(string viewName)
    {
      
        
            var categories = _categoryRepository.GetAll();

            var responseViewModel = new ProductAddResponseViewModel
            {
                Categories = categories
            };

            return View(viewName, responseViewModel);
        
    }

    #region Delete
    [HttpGet("delete")]
    public IActionResult Delete(int id)
    {
        Product product = _productRepository.GetById(id);

        if (product == null)
        {
            return NotFound();
        }
        _productRepository.RemoveById(id);


        return RedirectToAction("Products");
    }

    #endregion

    protected override void Dispose(bool disposing)
    {
        _productRepository.Dispose();
        _categoryRepository.Dispose();

        base.Dispose(disposing);
    }

}
