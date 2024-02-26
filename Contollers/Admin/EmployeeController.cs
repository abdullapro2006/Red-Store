using Microsoft.AspNetCore.Mvc;
using RedStore.Database.Repositories;

namespace RedStore.Contollers.Admin;

[Route("admin/employees")]
public class EmployeeController : Controller
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly ILogger<ProductController> _logger;
    public EmployeeController()
    {
        _employeeRepository = new EmployeeRepository();


        var factory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        _logger = factory.CreateLogger<ProductController>();
    }

    public IActionResult Index()
    {
        return View();
    }

    #region Products
    [HttpGet]
    public IActionResult Employees()
    {
        return View("Views/Admin/Employee/Employees.cshtml", _employeeRepository.GetAll());
    }

    #endregion

}
