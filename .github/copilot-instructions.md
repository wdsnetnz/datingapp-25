# Copilot instructions for DatingApp

These concise notes are intended to get an AI coding agent productive in this repository quickly. Focus on discoverable, actionable patterns and examples.

## Big picture
- **Two-project repo:** `API` (.NET 10 minimal API + controllers, services, repositories, EF Core) and `client` (Angular v21+ frontend).
- **Data flow:** client → HTTP calls to `API` controllers → `Services` (business logic) → `Repositories` → `AppDbContext` → SQLite (datingapp.db / EF migrations).

## Key files to read first
- `API/Program.cs` — app startup, DI registrations, OpenAPI configuration.
- `API/Controllers/MembersController.cs` — HTTP endpoints for members (currently empty stub).
- `API/Services/DatingService.cs` — service layer; `GetMembersAsync` is currently unimplemented.
- `API/Repositories/DatingRepository.cs` — repository pattern; note it can create its own `AppDbContext` with a SQLite file.
- `API/Data/AppDbContext.cs` and `API/Data/Migrations/` — EF Core model and migrations.
- `client/.claude/CLAUDE.md` and `client/README.md` — frontend conventions and run instructions.

## Project-specific patterns & conventions
- **Repository vs DI:** `DatingRepository` contains two constructors: one that creates its own `AppDbContext` (uses `Data Source=datingapp.db`) and one that accepts a context (DI). The app currently registers `IDatingRepository` and `IDatingService` in `Program.cs` but the `AddDbContext` call is commented out. Prefer using DI when adding new features; if you enable DI, uncomment or add `builder.Services.AddDbContext<AppDbContext>(...)` and provide a connection string.
- **SQLite is the default persistence:** migrations exist under `API/Data/Migrations`. The runtime code may create the DB automatically (see `DatingRepository`), so be careful not to overwrite local developer databases unintentionally.
- **OpenAPI in dev:** `builder.Services.AddOpenApi()` and `app.MapOpenApi()` are used — Swagger/OpenAPI is exposed only when `IsDevelopment()`.
- **Controller routing:** controllers use `[Route("api/[controller]")]` (e.g., `api/members`).

## Typical developer workflows (commands)
- Build the whole solution: `dotnet build DatingApp.sln` or `dotnet build` from repo root.
- Run the API: `dotnet run --project API` (serves the API; check console for port).
- Run the client: from `client/` run `npm install` then `ng serve` (default: http://localhost:4200/).
- EF migrations (if editing the model): inside `API/` (or pass --project/--startup-project):
  - `dotnet tool restore` (if using local tools)
  - `dotnet tool install --global dotnet-ef` (if `dotnet ef` missing)
  - `dotnet ef migrations add <Name> --project API --context AppDbContext`
  - `dotnet ef database update --project API --context AppDbContext`

## What to watch for when modifying code
- If you change DI or `AppDbContext` registration, remove the repository's internal DB creation only after confirming migration/update flows work for contributors.
- There is no CORS middleware configured — if the client is served from a different origin during development, add `builder.Services.AddCors(...)` and `app.UseCors(...)` in `Program.cs`.
- `DatingService.GetMembersAsync` throws `NotImplementedException` — implement business logic by calling the repository (see `GetMemberAsync` for example usage).

## Frontend rules (from client/.claude/CLAUDE.md)
- Follow Angular v20+ idioms: standalone components, signals for state, `computed()` for derived state, `inject()` over constructor DI where appropriate.
- Templates prefer native control flow (`@if/@for`) and avoid heavy logic in templates.

## Good-first tasks for an AI agent
- Implement `GetMembersAsync` in `API/Services/DatingService.cs` and add matching endpoints in `API/Controllers/MembersController.cs`.
- Add minimal CORS enabling for developer convenience and document the change.
- Optionally uncomment and configure `AddDbContext` in `Program.cs` to centralize DB config; update `appsettings.json` with a `DefaultConnection` if you do.

If any section is unclear or you want these instructions extended (examples, code snippets, or automated CI steps), tell me which area to expand. I'll iterate. 
