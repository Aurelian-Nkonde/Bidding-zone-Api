# Bidding Zone API

A backend service for an online auction / marketplace platform. Users list items for sale, other users place time-boxed bids on them, and the highest bidder is tracked automatically as the auction progresses.

Built with **ASP.NET Core 10**, **Entity Framework Core**, and **PostgreSQL**.

## Features

- **Users** — registration, profile updates, address management, and JWT-based login (passwords hashed with BCrypt).
- **Items** — create, update, and manage auction listings with a starting price, duration, and status (`Active`, `Sold`, `Canceled`).
- **Bids** — place bids on active items; the highest bidder for an item is recalculated automatically on every new bid.
- **Validation** — all request payloads are validated with FluentValidation before hitting the database.
- **Structured logging** — request and application logs via Serilog, written to the console and to `Logs/log.txt`.

## Tech Stack

| Layer          | Technology                                  |
|----------------|----------------------------------------------|
| Framework      | ASP.NET Core 10 (Web API)                    |
| ORM            | Entity Framework Core 10 (Npgsql provider)   |
| Database       | PostgreSQL                                   |
| Validation     | FluentValidation                             |
| Auth           | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) |
| Password hashing | BCrypt.Net-Next                            |
| Logging        | Serilog (console + rolling file sink)        |

## Project Structure

```
Controllers/    API endpoints (Users, Items, Bids)
Services/       Business logic, one service per domain area
Interfaces/     Service contracts (IUsersService, IItemsService, IBidService)
Models/         EF Core entities and enums
Dtos/
  Request/      Incoming request payloads
  Response/     Outgoing response shapes
  Validators/   FluentValidation rules per DTO
AppContext/     EF Core DbContext and model configuration
Configuration/  Strongly-typed options (JwtOptions)
Migrations/     EF Core migrations
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A running PostgreSQL instance

### 1. Configure the database connection

Update the connection string in `appsettings.json` (or override it via environment variables / `appsettings.Development.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=bidding_zone_api;Username=postgres;Password=your_password;"
}
```

### 2. Configure JWT settings

```json
"Jwt": {
  "Issuer": "https://bidding-zone-api.com",
  "Key": "a-long-random-secret-key",
  "Audience": "bidding_zone_fe",
  "ExpiresAt": 60
}
```

> Replace the sample key with a strong, private secret before deploying — do not commit real secrets to source control.

### 3. Apply migrations

```bash
dotnet ef database update
```

This creates the schema and seeds it with sample users, items, and bids for local development.

### 4. Run the API

```bash
dotnet run
```

By default the API listens on `http://localhost:5074` (see `Properties/launchSettings.json`). In development mode, an OpenAPI document is available at `/openapi/v1.json`.

A `bidding-zone-api.http` file is included with sample requests for use with the VS Code REST Client or a similar tool.

## API Reference

Base path: `/api`

### Users

| Method | Route                          | Auth | Description                                   |
|--------|----------------------------------|:----:|------------------------------------------------|
| POST   | `/Users`                       | 🔒  | Register a new user                            |
| POST   | `/Users/login`                 |      | Authenticate and receive a JWT                 |
| GET    | `/Users/me`                    | 🔒  | Get the currently authenticated user           |
| GET    | `/Users`                       | 🔒  | List all users                                 |
| GET    | `/Users/{id}`                  |      | Get a user by id                               |
| PUT    | `/Users/{id}/update`           | 🔒  | Update a user's profile                        |
| PUT    | `/Users/{id}/update/address`   | 🔒  | Update a user's address                        |

### Items

| Method | Route                         | Description                                          |
|--------|--------------------------------|-------------------------------------------------------|
| GET    | `/Items`                      | List all items                                         |
| GET    | `/Items/{id}`                 | Get an item by id                                      |
| POST   | `/Items`                      | Create a new item listing                              |
| PUT    | `/Items/update/{id}`          | Update an item's title, description, price, or duration |
| PUT    | `/Items/update/{id}/status`   | Change an item's status (`Active`, `Sold`, `Canceled`) |

### Bids

| Method | Route                | Description                                                     |
|--------|-----------------------|-------------------------------------------------------------------|
| GET    | `/Bids`               | List all bids                                                     |
| GET    | `/Bids/{id}`          | Get a bid by id                                                   |
| GET    | `/Bids/user/{id}`     | Get bids for a specific user                                      |
| POST   | `/Bids`               | Place a new bid on an item                                        |
| PUT    | `/Bids/status/{id}`   | Change a bid's status (e.g. cancel it)                             |
| DELETE | `/Bids/{id}`          | Permanently delete a bid (only by its owner)                      |

### Enums

Enums are accepted as **case-insensitive strings** in request bodies (e.g. `"status": "Active"`), but are serialized as their **numeric ordinal** in response bodies, since no string enum converter is registered.

| Enum          | Values (in ordinal order)                                                                 |
|---------------|---------------------------------------------------------------------------------------------|
| `Gender`      | `Male`, `Female`                                                                             |
| `Province`    | `Western_cape`, `Northen_cape`, `Mpumalanga`, `Limpopo`, `Gauteng`, `Kwazulu_natal`, `North_west`, `Eastern_cape` |
| `ItemStatus`  | `Active`, `Sold`, `Canceled`                                                                 |
| `BidStatus`   | `Rejected`, `Accepted`, `Active`, `Canceled`, `Closed`                                       |
| `Bidding_times` (item duration on create) | `Minutes_5`, `Minutes_10`, `Minutes_30`, `Hours_1`               |

## Known Limitations

This is an evolving project; a few rough edges are worth knowing about before building against it:

- **Items and Bids endpoints are not yet authorization-protected.** Mutating endpoints on `ItemsController` and `BidsController` currently act on a hardcoded user id rather than the authenticated caller — only the `Users` controller enforces `[Authorize]`.
- `GET /Bids/user/{id}` currently returns **all** bids rather than filtering by user; filter client-side by `userId` until this is fixed.
- `PUT /Bids/{id}` does not update an existing bid — it creates a new one. Use `PUT /Bids/status/{id}` to change a bid's status instead.

## Logging

Logs are written using Serilog to both the console and `Logs/log.txt`, configured in `appsettings.json`. Log level and sinks can be adjusted there without code changes.
