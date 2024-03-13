using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Services.Abstract;

namespace RedStore.Services.Concretes;

public class BasketService : IBasketService
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public BasketService(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }

    public void CreateOrIncrementQuantity(int productId, int sizeId, int colorId, User user)
    {
        var basketProduct = _redStoreDbContext.BasketProducts
            .FirstOrDefault(bp =>
                bp.UserId == user.Id &&
                bp.ProductId == productId &&
                bp.SizeId == sizeId &&
                bp.ColorId == colorId);

        if (basketProduct != null)
        {
            basketProduct.Quantity++;
            return;
        }

        basketProduct = new BasketProduct
        {
            ProductId = productId,
            SizeId = sizeId,
            ColorId = colorId,
            User = user,
            Quantity = 1
        };

        _redStoreDbContext.BasketProducts.Add(basketProduct);
    }
}
