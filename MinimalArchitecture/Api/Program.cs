using Domain.Ports.Primary;
using Domain.Ports.Secondary;
using Domain.Services;
using JsonRepository;
using XMLRepository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddScalarTransformers(); // Habilita las extensiones de Scalar
});

string pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");
string xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.xml");

builder.Services.AddKeyedTransient<IRepository>(
    "json", (_, _) => new ProductRepository(pathFile));

builder.Services.AddKeyedTransient<IRepository>(
    "xml", (_, _) => new XMLProductRepository(xmlFile));

builder.Services.AddKeyedTransient<IService>(
    "json", (provider, _) =>
        new ProductService(provider.GetRequiredKeyedService<IRepository>("json")));

builder.Services.AddKeyedTransient<IService>(
    "xml", (provider, _) =>
        new ProductService(provider.GetRequiredKeyedService<IRepository>("xml")));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

#region "************ JSON ************"

app.MapGet("/products",
    ([FromKeyedServices("json")] IService service) => service.GetAll());

app.MapPost("/products", (string name, decimal price,
    [FromKeyedServices("json")] IService service) =>
{
    service.Register(name, price);
    return Results.Created();
});

#endregion

#region "************ XML ************"

app.MapGet("/productsXML",
    ([FromKeyedServices("xml")] IService service) => service.GetAll());

app.MapPost("/productsXML", (string name, decimal price,
    [FromKeyedServices("xml")] IService service) =>
{
    service.Register(name, price);
    return Results.Created();
});

#endregion

app.Run();