using API.Contracts;
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
    return Results.Ok(items.Select(ItemResponse.FromDomain).ToArray());
})
.WithName("GetItems");

app.MapGet("/items/{id:int}", async (
    int id,
    IService<Item> service,
    CancellationToken cancellationToken) =>
{
    var item = await service.GetByIdAsync(id, cancellationToken);
    return item is null
        ? Results.NotFound()
        : Results.Ok(ItemResponse.FromDomain(item));
})
.WithName("GetItemById");

app.MapPost("/items", async (
    CreateItemRequest request,
    IService<Item> service,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.Problem(
            detail: "Title is required.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    await service.AddAsync(new Item(0, request.Title, false), cancellationToken);
    return Results.Created();
})
.WithName("CreateItem");

app.MapGet("/notes", async (IService<Note> service, CancellationToken cancellationToken) =>
{
    var notes = await service.GetAsync(cancellationToken);
    return Results.Ok(notes.Select(NoteResponse.FromDomain).ToArray());
})
.WithName("GetNotes");

app.MapGet("/notes/{id:int}", async (
    int id,
    IService<Note> service,
    CancellationToken cancellationToken) =>
{
    var note = await service.GetByIdAsync(id, cancellationToken);
    return note is null
        ? Results.NotFound()
        : Results.Ok(NoteResponse.FromDomain(note));
})
.WithName("GetNoteById");

app.MapPost("/items/{itemId:int}/notes", async (
    int itemId,
    CreateNoteRequest request,
    IService<Item> itemService,
    IService<Note> noteService,
    CancellationToken cancellationToken) =>
{
    var item = await itemService.GetByIdAsync(itemId, cancellationToken);

    if (item is null)
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(request.Message))
    {
        return Results.Problem(
            detail: "Message is required.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    if (request.Message.Length > 100)
    {
        return Results.Problem(
            detail: "Message must be 100 characters or fewer.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    if (request.Color?.Length > 6)
    {
        return Results.Problem(
            detail: "Color must be 6 characters or fewer.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    await noteService.AddAsync(
        new Note(0, itemId, request.Message, request.Color),
        cancellationToken);
    return Results.Created();
})
.WithName("CreateNote");

app.Run();
