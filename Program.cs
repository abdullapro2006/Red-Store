using Microsoft.AspNetCore.Mvc;
using RedStore.Services;

namespace RedStore
{
    public class Program {
    
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

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute("default", "{controller}/{action}");

            app.Run();

        }
    }



}