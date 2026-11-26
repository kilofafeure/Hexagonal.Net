using Application;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddScalarTransformers(); // Habilita las extensiones de Scalar
});

builder.Services.AddScoped<IRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IService<Item>, ItemService>();

builder.Services.AddScoped<IRepository<Note>, NoteRepository>();
builder.Services.AddScoped<IService<Note>, NoteService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' it's not configured.");

builder.Services.AddDbContext<ItemsDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/items", async (IService<Item> service, CancellationToken cancellationToken) =>
{
    var items = await service.GetAsync(cancellationToken);
    return Results.Ok(items);
})
.WithName("GetItems");

app.MapPost("/items", async (string title, IService<Item> service, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(title))
        return Results.Problem("Title is mandatory.");

    await service.AddAsync(new Item(0, title, false), cancellationToken);
    return Results.Created();
})
.WithName("CreateItem");

app.MapGet("/notes", async (IService<Note> service, CancellationToken cancellationToken) =>
{
    var notes = await service.GetAsync(cancellationToken);
    return Results.Ok(notes);
})
.WithName("GetNotes");

app.MapPost("/notes", async (Note note, IService<Note> service, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(note.Message))
        return Results.Problem("Message is mandatory.");

    if (note.Message.Length > 100)
        return Results.Problem("Message can't be longer than 100 characters.");

    await service.AddAsync(note, cancellationToken);
    return Results.Created();
})
.WithName("CreateNote");

app.Run();
