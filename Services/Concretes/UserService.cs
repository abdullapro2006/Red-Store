using RedStore.Database;
using RedStore.Database.DomainModels;
using RedStore.Services.Abstract;

namespace RedStore.Services.Concretes;

public class UserService : IUserService
{
    private readonly RedStoreDbContext _redStoreDbContext;
    private readonly IHttpContextAccessor _httpcontextAccessor;
    private User _currentUser = null;

    public User CurrentUser
    {
        get { 
            
            return _currentUser ??= GetCurrentLoggedUser(); 
        }
    }

    public bool IsAuthenticateed {
        get
        {
            return _httpcontextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
         
    }

       


    public UserService(RedStoreDbContext redStoreDbContext, IHttpContextAccessor httpcontextAccessor)
    {
        _redStoreDbContext = redStoreDbContext;
        _httpcontextAccessor = httpcontextAccessor;
    }

    public User GetCurrentLoggedUser()
    {
        var currentUserId = _httpcontextAccessor.HttpContext.User.FindFirst(c => c.Type == "Id").Value;

        return _redStoreDbContext.Users.Single(u => u.Id == Convert.ToInt32(currentUserId));
    }

    public string GetFullName(User user)
    {
        return user.Name + " " + user.LastName;
    }

    public string GetCurrentUserFullName()
    {
        return GetFullName(CurrentUser);
    }

    public bool IsUserSeeded(User user)
    {
        return user.Id < 0;
    }
}
