using Microsoft.EntityFrameworkCore;
using RedStore.Contracts;
using RedStore.Database;
using RedStore.Services.Abstract;
using RedStore.Services.Concretes;

namespace RedStore
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //DatabaseService databaseService = new DatabaseService();
            //databaseService.InitializeTables();
            //databaseService.Dispose();

            //boiler plate 
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.
                AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            builder.Services
                .AddAuthentication("Cookies")
                .AddCookie("Cookies", o =>
                {
                    o.LoginPath = "/auth/login";
                    o.LogoutPath = "/home/index";
                });

            builder.Services
                .AddHttpContextAccessor()
                .AddSingleton<IFileService,FileService>()
                 .AddScoped<IUserService, UserService>()
                 .AddScoped<IProductService, ProductService>()
                 .AddScoped<IBasketService, BasketService>()
                .AddDbContext<RedStoreDbContext>(o =>
                {
                    o.UseNpgsql(DatabaseConstants.CONNECTION_STRING);
                });

            var app = builder.Build();

            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

            app.Run();

        }
    }



}