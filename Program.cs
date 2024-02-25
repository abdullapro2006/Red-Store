using Microsoft.AspNetCore.Mvc;

namespace RedStore
{
    public class Program {
    
        public static void Main(string[] args)
        {
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