using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class Color : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ProductColor> ProductColors { get; set; }
}
