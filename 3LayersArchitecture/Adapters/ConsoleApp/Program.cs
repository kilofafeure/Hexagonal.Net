using ApplicationComponent;
using ApplicationComponent.Interfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' no está configurada en appsettings.json.");

await using var serviceProvider = new ServiceCollection()
    .AddDbContext<ItemsDbContext>(options => options.UseSqlServer(connectionString))
    .AddScoped<IRepository, ItemRepository>()
    .AddScoped<IService, ItemService>()
    .BuildServiceProvider();

await using var scope = serviceProvider.CreateAsyncScope();
var itemService = scope.ServiceProvider.GetRequiredService<IService>();
