using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class Size : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
