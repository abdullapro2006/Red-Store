using RedStore.Database.DomainModels;

namespace RedStore.ViewModels.Product;

public class ProductUpdateResponseViewModel : BaseProductViewModel
{
    public int Id { get; set; }
    public List<Category> Categories { get; set; }
}
