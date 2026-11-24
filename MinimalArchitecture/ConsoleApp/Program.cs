using Domain.Ports.Primary;
using Domain.Ports.Secondary;
using Domain.Services;
using JsonRepository;
using Microsoft.Extensions.DependencyInjection;
using System;

string pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");

var services = new ServiceCollection();
services.AddTransient<IRepository>(provider => new ProductRepository(pathFile));
services.AddTransient<IService, ProductService>();

using var serviceProvider = services.BuildServiceProvider();
IService productService = serviceProvider.GetRequiredService<IService>();

while (true)
{
    try
    {
        Console.WriteLine("\nSelect an option");
        Console.WriteLine("1 - Add product");
        Console.WriteLine("2 - Show all products");
        Console.WriteLine("3 - Exit");
        Console.WriteLine("Option: ");

        string option = Console.ReadLine() ?? string.Empty;

        switch (option)
        {
            case "1":
                Console.Write("Set a product: ");
                string name = Console.ReadLine() ?? string.Empty;
                Console.Write("Set a price: ");

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.Write("\nProduct name cannot be empty");
                    break;
                }

                if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                {
                    Console.Write("\nIncorrect price");
                    break;
                }

                productService.Register(name, price);
                break;
            case "2":
                Console.Write("\nStored products");
                foreach (var product in productService.GetAll()) {
                    Console.WriteLine($"- {product.Name}: {product.Price}");
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
        Console.WriteLine($"ERROR: { ex.Message }");
    }
}