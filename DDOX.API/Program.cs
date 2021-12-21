using DDOX.API;
using DDOX.API.Repository.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
    public static void Main(string[] args)
    {

        var confg = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(confg).CreateLogger();
        try
        {
            Log.Information("Start To Build The Application");
            var host = CreateHostBuilder(args).Build();

            Log.Information("Start To Build The Data Base");
            CreateDbIfNotExists(host);

            Log.Information("Start To Run The Application");
            host.Run();
        }
        catch (Exception ex)
        {

            Log.Fatal(ex, "Fail to run the application");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void CreateDbIfNotExists(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error While creating database");
                Console.WriteLine(ex.Message);
            }
        }
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

