using RedStore.Database.DomainModels;

namespace RedStore.ViewModels;

public class ProductAddResponseViewModel : BaseProductViewModel
{
    public List<Category> Categories { get; set; }
}
