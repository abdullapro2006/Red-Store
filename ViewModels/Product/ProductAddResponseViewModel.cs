using RedStore.Database.DomainModels;

namespace RedStore.ViewModels.Product;

public class ProductAddResponseViewModel : BaseProductViewModel
{
    public List<Category> Categories { get; set; }
}
