using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Services.Abstract;

namespace RedStore.Services.Concretes;

public class UserService : IUserService
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public UserService(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }

    public User GetCurrentLoggedUser()
    {
        return _redStoreDbContext.Users.Single(u => u.Id == -1);
    }
}
