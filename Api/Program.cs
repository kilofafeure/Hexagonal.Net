using Domain.Ports.Primary;
using Domain.Ports.Secondary;
using Domain.Services;
using JsonRepository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddScalarTransformers(); // Habilita las extensiones de Scalar
});

string pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");

builder.Services.AddTransient<IRepository>(provider => new ProductRepository(pathFile));
builder.Services.AddTransient<IService, ProductService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/products", (IService service) =>
{
    return service.GetAll();
}).WithName("GetProducts");

app.MapPost("/products", (string name, decimal price, IService service) =>
{
    service.Register(name, price);
    return Results.Created();
}).WithName("AddProduct");

app.Run();

