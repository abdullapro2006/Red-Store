using RedStore.Database.DomainModels;

namespace RedStore.ViewModels;

public class ProductUpdateResponseViewModel : BaseProductViewModel
{
    public int Id { get; set; }
    public List<Category> Categories { get; set; }
}
