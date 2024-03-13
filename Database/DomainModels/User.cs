using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
}
