using RedStore.Database.DomainModels;
using System.Drawing;

namespace RedStore.Services.Abstract;

public interface IProductService
{
    Database.DomainModels.Color GetDefaultColor(int productId);
    Database.DomainModels.Size GetDefaultSize(int productId);

    int GetDefaultColorId(int productId);
    int GetDefaultSizeId(int productId);
}
