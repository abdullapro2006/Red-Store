using Microsoft.EntityFrameworkCore;
using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Services.Abstract;

namespace RedStore.Services.Concretes;

public class ProductService : IProductService
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public ProductService(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }

    public Color GetDefaultColor(int productId)
    {
        return _redStoreDbContext.ProductColors
            .Where(pc => pc.ProductId == productId)
            .Include(pc => pc.Color)
            .First()
            .Color;
    }
    public int GetDefaultColorId(int productId)
    {
        var color = GetDefaultColor(productId);
        return color.Id;
    }

    public Size GetDefaultSize(int productId)
    {
        return _redStoreDbContext.ProductSizes
           .Where(pc => pc.ProductId == productId)
           .Include(pc => pc.Size)
           .First()
           .Size;
    }

    public int GetDefaultSizeId(int productId)
    {
        var size = GetDefaultSize(productId);
        return size.Id;
    }

  
}
