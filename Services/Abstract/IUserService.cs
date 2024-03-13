using RedStore.Database.DomainModels;

namespace RedStore.Services.Abstract;

public interface IUserService
{
    User GetCurrentLoggedUser();
}
