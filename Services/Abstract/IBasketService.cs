using RedStore.Database.DomainModels;

namespace RedStore.Services.Abstract;

public interface IBasketService
{
    void CreateOrIncrementQuantity(int productId, int sizeId, int colorId, User user);
}
