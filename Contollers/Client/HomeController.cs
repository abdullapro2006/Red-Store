﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedStore.Database;

namespace RedStore.Contollers.Client;

public class HomeController : Controller
{
    private readonly RedStoreDbContext _dbContext;

    public HomeController()
    {
        _dbContext = new RedStoreDbContext();
    }

    public ViewResult Index()
    {
        return View(_dbContext.Products.ToList());
    }
    public ViewResult About()
    {
        return View();
    }

    protected override void Dispose(bool disposing)
    {
        _dbContext.Dispose();

        base.Dispose(disposing);
    }
}
