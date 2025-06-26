using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ModernDesign;
using ModernDesign.DataBase;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configuration = LoadConfiguration();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, configuration);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        var logger = ServiceProvider.GetRequiredService<ILogger<App>>();
        logger.LogInformation("Application started");
    }

    private static IConfiguration LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        return builder.Build();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Trace);
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<MainViewModel>();
        services.AddScoped<GroupCollectionViewModel>();
        services.AddScoped<CourseCollectionViewModel>();
        services.AddScoped<StudentCollectionViewModel>();
        services.AddScoped<TeacherCollectionViewModel>();
        services.AddScoped<MainCollectionViewModel>();
        services.AddSingleton<ICsvManager, CsvManager>();
        services.AddSingleton<IFileDialogService, FileDialogService>();
        services.AddSingleton<MainWindow>();
    }
}