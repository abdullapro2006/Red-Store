using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class ProductSize : IEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int SizeId { get; set; }
    public Size Size { get; set; }
}
