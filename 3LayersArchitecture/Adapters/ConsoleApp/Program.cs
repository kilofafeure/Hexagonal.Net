using Application;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Mapping;
using Repository.Model;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' it's not configured appsettings.json.");

await using var serviceProvider = new ServiceCollection()
    .AddDbContext<ItemsDbContext>(options => options.UseSqlServer(connectionString))
    .AddScoped<IMapper<ItemModel, Item>, ItemMapper>()
    .AddScoped<IRepository<Item>, ItemRepository>()
    .AddScoped<IService<Item>, ItemService>()
    .BuildServiceProvider();

await using var scope = serviceProvider.CreateAsyncScope();
var itemService = scope.ServiceProvider.GetRequiredService<IService<Item>>();

while (true) {
    try
    {
        Console.WriteLine("\nSelect an option");
        Console.WriteLine("1 - Add task");
        Console.WriteLine("2 - Show all tasks");
        Console.WriteLine("3 - Exit");
        Console.WriteLine("Option: ");

        string option = Console.ReadLine() ?? string.Empty;

        switch (option)
        {
            case "1":
                Console.Write("Set a task: ");
                string title = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.Write("\nTitle cannot be empty");
                    break;
                }

                await itemService.AddAsync(new Item(0, title, false));
                break;
            case "2":
                Console.WriteLine("\nGetting tasks...");
                var items = await itemService.GetAsync();

                Console.WriteLine("\nStored tasks: ");
                foreach (var task in items)
                {
                    Console.WriteLine($"[{(task.IsCompleted ? "x" : " ")}] {task.Title}");
                }
                break;
            case "3":
                Console.Write("\nExiting...");
                return;
            default:
                Console.Write("\nIncorrect option");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}