using RedStore.Database;

namespace RedStore.Services;

public class EmployeeService : IDisposable
{
    private readonly RedStoreDbContext _redStoreDbContext;

    public EmployeeService()
    {
        _redStoreDbContext = new RedStoreDbContext();
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
