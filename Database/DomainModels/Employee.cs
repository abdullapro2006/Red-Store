using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class Employee : IEntity
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FatherName { get; set; }
    public string Pin { get; set; }
    public string Email { get; set; }
    public int DeparmentId { get; set; }
    public Deparment Deparment { get; set; }
}
