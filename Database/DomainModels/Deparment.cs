using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class Deparment : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
