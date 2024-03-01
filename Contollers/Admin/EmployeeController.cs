using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Services;
using RedStore.ViewModels;
using RedStore.ViewModels.Employee;
using RedStore.ViewModels.Product;

namespace RedStore.Contollers.Admin;

[Route("admin/employees")]
public class EmployeeController : Controller
{
    private readonly RedStoreDbContext _dbContext;
    private readonly ILogger<EmployeeController> _logger;
    private readonly EmployeeService _employeeService;
    public EmployeeController(RedStoreDbContext redStoreDbContext, 
        EmployeeService employeeService,
        ILogger<EmployeeController> logger)
    {
        _dbContext = redStoreDbContext;
        
        _employeeService = employeeService;
        _logger = logger;


    }


    #region Employees
    [HttpGet]
    public IActionResult Employees()
    {
        var employeesQuery = _dbContext.Employees.
            Where(e => !e.IsDeleted)
            .OrderBy(e => e.Name)
            .AsQueryable();

        var sql = employeesQuery.ToQueryString();

        return View("Views/Admin/Employee/Employees.cshtml", employeesQuery.ToList());
    }

    #endregion

    #region Add

    [HttpGet("add")]
    public IActionResult Add()
    {
        var model = new EmployeeViewModel
        {
           Departments = _dbContext.Departments.ToList()
        };

        return View("Views/Admin/Employee/EmployeeAdd.cshtml", model);
    }



    [HttpPost("add")]
    public IActionResult Add(EmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return PrepareValidationView("Views/Admin/Employee/EmployeeAdd.cshtml");
        }

        var query = _dbContext.Departments
            .Where(d => d.Id == model.DeparmentId)
            .AsQueryable()
            .ToQueryString();


       
            var department = _dbContext.Departments.FirstOrDefault(d => d.Id == model.DeparmentId);

            if (department == null)
            {
                ModelState.AddModelError("departmentId", "Department doesn't exist");

                return PrepareValidationView("Views/Admin/Product/ProductAdd.cshtml");
            }
     

        var employee = new Employee
        {
            Name = model.Name,
            Code = _employeeService.GenerateAndGetCode(),
            DepartmentId = model.DeparmentId,
            Email = model.Email,
            FatherName = model.FatherName,
            Pin = model.Pin,
            Surname = model.Surname,
        };

        try
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }
        catch (PostgresException e)
        {
            _logger.LogError(e, "Postgresql Exception");

            throw e;

        }




        return RedirectToAction("Employees");
    }

    #endregion

    #region Delete
    [HttpGet("delete")]
    public IActionResult Delete(string code)
    {
        Employee employee = _dbContext.Employees.FirstOrDefault(e => e.Code == code);

        if (employee == null)
        {
            return NotFound();
        }

        employee.IsDeleted = true;

        _dbContext.Employees.Update(employee);
        _dbContext.SaveChanges();



        return RedirectToAction("Employees");
    }

    #endregion

    private ViewResult PrepareValidationView(string viewName)
    {


        var departments = _dbContext.Departments.ToList();

        var responseViewModel = new EmployeeViewModel
        {
           Departments = departments
        };

        return View(viewName, responseViewModel);

    }

    protected override void Dispose(bool disposing)
    {
        _dbContext.Dispose();
        _employeeService.Dispose();

        base.Dispose(disposing);
    }

}
