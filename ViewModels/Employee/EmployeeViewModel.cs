using RedStore.Database.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedStore.ViewModels.Employee;

public class EmployeeViewModel
{
    public int? Id { get; set; }

    [NotMapped]
    public string Code { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FatherName { get; set; }
    public string Pin { get; set; }
    public string Email { get; set; }
    public int DeparmentId { get; set; }
    public List<Department> Departments { get; set; }
}
