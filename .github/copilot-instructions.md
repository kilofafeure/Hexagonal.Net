# Copilot instructions for Hexagonal.Net

## Repository overview

This is a small .NET 10 console application structured around ports-and-adapters/hexagonal architecture.

- `Domain/` is the core project. It contains the `Product` entity, primary/secondary port interfaces, and `ProductService` application logic.
- `JsonRepository/` is the JSON persistence adapter implementing `Domain.Ports.Secondary.IRepository`.
- `ConsoleApp/` is the composition root and user interface. It configures dependency injection and runs the console menu.
- `Hexagonal.slnx` is the solution file. It groups the three projects under `MinimalArchitecture`.

Dependency direction: `ConsoleApp` depends on `Domain` and `JsonRepository`; `JsonRepository` depends on `Domain`; `Domain` does not reference the adapters or the console project.

## Build and run commands

Run commands from the repository root.

```powershell
dotnet build Hexagonal.slnx
dotnet run --project ConsoleApp/ConsoleApp.csproj
dotnet clean Hexagonal.slnx
dotnet restore Hexagonal.slnx
```

The solution currently has no test project and no test packages or lint configuration. Do not invent a test command or run one that only works after adding test infrastructure.

Use the smallest project build while working on an isolated project:

```powershell
dotnet build Domain/Domain.csproj
dotnet build JsonRepository/JsonRepository.csproj
dotnet build ConsoleApp/ConsoleApp.csproj
```

When no test project exists, there is also no single-test command to run.

## Architecture and data flow

- `IService` is the primary port exposed to the console application.
- `IRepository` is the secondary port used by the domain service for persistence.
- `ProductService.Register` creates a `Product` and delegates persistence through `IRepository.Save`.
- `ProductService.GetAll` delegates reads through `IRepository.GetAll`.
- `ProductRepository` reads and writes the JSON file, adding a newly registered product to the deserialized list before rewriting the file.

The console application's `IRepository` registration supplies `ProductRepository` with a file path. The default data file is:

```text
<AppDomain.CurrentDomain.BaseDirectory>/products.json
```

A different repository implementation can be substituted through the `IRepository` registration in `ConsoleApp/Program.cs`.

## Conventions specific to this repository

- Preserve the dependency boundaries: domain code must depend on domain interfaces and entities, not on `JsonRepository` or `ConsoleApp` types.
- Implement `IService` in domain application services and `IRepository` in infrastructure adapters.
- Use constructor injection for `ProductService` and `ProductRepository`, as established in the current primary constructors.
- Keep the service provider and runtime composition wiring in `ConsoleApp/Program.cs`; do not move it into `Domain`.
- Add new repository implementations under an adapter project and register them in `ConsoleApp/Program.cs` through `IRepository`.
- The persistence JSON is indented by `WriteIndented = true`. Reuse the cached `JsonSerializerOptions` instance in `ProductRepository` rather than constructing options on every `Save` call.
- Entity validation belongs in `Domain.Entities.Product` setters/constructor logic. Preserve `Guid`-based product identity and decimal prices.
- Console input is parsed and validated in `Program.cs`; invalid name or price input should remain a menu-loop error rather than terminate the application.
- All projects target `net10.0`, enable implicit usings, and enable nullable reference types.
