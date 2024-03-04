using RedStore.Database;
using RedStore.Services.Abstract;

namespace RedStore.Services.Concretes;

public class EmployeeServiceImp : IEmployeeService,IDisposable
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public EmployeeServiceImp(RedStoreDbContext redStoreDbContext)
    {
        _redStoreDbContext = redStoreDbContext;
    }



    public string GenerateAndGetCode()
    {
        var random = new Random();
        string numberPart;
        string code;



        do
        {
            numberPart = random.Next(1, 100000).ToString();

            code = $"E{numberPart.PadLeft(5, '0')}";

        } while (_redStoreDbContext.Employees.Any(e => e.Code == code));


        return code;
    }

    public void Dispose()
    {
        _redStoreDbContext.Dispose();
    }
}
