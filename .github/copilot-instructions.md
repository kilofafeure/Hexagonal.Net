# Copilot instructions for Hexagonal.Net

## Repository overview

This .NET 10 solution contains two architecture examples, grouped in `Hexagonal.slnx`:

- `MinimalArchitecture/` contains `Api`, `ConsoleApp`, `Domain`, `JsonRepository`, and `XMLRepository` projects. It demonstrates ports and adapters with alternative persistence adapters.
- `3LayersArchitecture/` contains `Domain`, `Application`, `Repository`, and API/console adapters. The API currently exposes `Item` and `Note` operations; the repository uses Entity Framework Core and SQL Server.

Keep the examples distinct. Do not move types or registrations between them unless the request explicitly asks for it.

### Dependency direction

- In `MinimalArchitecture`, the application/domain core must not depend on API, console, or persistence implementations. Adapters depend inward on the core contracts.
- In `3LayersArchitecture`, `Domain` is independent; `Application` depends on `Domain`; `Repository` depends on `Domain`; and `Adapters/API` and `Adapters/ConsoleApp` compose the application and infrastructure.
- Keep dependency-injection registrations and runtime composition in adapter entry points, not in `Domain` or `Application`.

## Build, run, and test

Run commands from the repository root. Build the solution to validate changes across both examples:

```powershell
dotnet build Hexagonal.slnx
dotnet clean Hexagonal.slnx
dotnet restore Hexagonal.slnx
```

Run the relevant executable project when needed:

```powershell
dotnet run --project MinimalArchitecture/ConsoleApp/ConsoleApp.csproj
dotnet run --project MinimalArchitecture/Api/Api.csproj
dotnet run --project 3LayersArchitecture/Adapters/ConsoleApp/ConsoleApp.csproj
dotnet run --project 3LayersArchitecture/Adapters/API/API.csproj
```

For an isolated change, build the smallest affected project. The solution currently has no test project or test packages; do not invent or run a test command that depends on adding test infrastructure.

## `3LayersArchitecture` conventions

- `IService<TEntity>` is the application-facing port; `IRepository<TEntity>` is the persistence port. Keep their generic operations and `CancellationToken` parameters consistent through services and repositories.
- Implement application behavior in `Application` services and persistence in `Repository`; API endpoints should depend on `IService<TEntity>`, not on EF Core or repository implementations.
- Register `ItemsDbContext`, services, and repositories in the adapter composition root with scoped lifetimes.
- Keep `Item` and `Note` validation in the domain entities. Avoid exposing domain entities as HTTP request DTOs; use request types in the API adapter.
- Notes belong to an Item. Use the nested route `/items/{itemId}/notes` for note creation and verify that the parent Item exists before persisting a Note.
- Return client error responses for invalid request data and `404 Not Found` for missing resources; do not turn validation failures into server errors.
- Use `AsNoTracking()` for read-only EF Core queries, pass cancellation tokens to EF operations, and use UTC timestamps for persisted creation times.
- `ItemsDbContext` maps `ItemModel` and `NoteModel`; preserve the required Note-to-Item foreign key relationship and cascade behavior unless a requirement changes it.
- Keep the configured connection string in the API's `appsettings.json` unless asked to change configuration. Do not replace it with a guessed value.

## `MinimalArchitecture` conventions

- Preserve the ports-and-adapters dependency boundaries and use the repository implementation registered by the relevant adapter composition root.
- Keep persistence-specific behavior in `JsonRepository` or `XMLRepository`; keep the domain/application core independent of their file formats and implementation details.
- Reuse existing serialization options and repository helpers rather than recreating configuration for each operation.

## General conventions

- All projects target `net10.0`, enable implicit usings, and enable nullable reference types.
- Follow existing naming, formatting, and composition patterns in the specific architecture being changed. Avoid unrelated refactoring.
