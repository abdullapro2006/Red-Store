using RedStore.Database.DomainModels;

namespace RedStore.Services.Abstract;

public interface IUserService
{
    User CurrentUser { get; }
    string GetFullName(User user);
    bool IsAuthenticateed {  get; }
    string GetCurrentUserFullName();
}
