Crea el archivo `README.md` en la raíz del repo con este contenido:

```markdown
# InventarySystem

Multimodular SaaS inventory management platform for small and medium businesses.
Built with Clean Architecture, designed as a Lego system where modules can be delivered alone or combined.

---

## Tech Stack

- **.NET 10** — REST API with Clean Architecture
- **PostgreSQL 16** — Relational database
- **Entity Framework Core 10** — ORM (Database-first)
- **Docker** — Local development environment
- **Swagger** — API documentation

---

## Project Structure

```
InventarySystem/
├── DataBase/
│   ├── docker-compose.yml
│   ├── init.sql
│   ├── .env
│   └── .env.example
└── InventarySystem.Api/
    ├── src/
    │   ├── Core/
    │   │   ├── Application/
    │   │   │   ├── DTOs/
    │   │   │   ├── Interfaces/
    │   │   │   └── Services/
    │   │   ├── Contracts/
    │   │   ├── Domain/
    │   │   │   ├── Entities/
    │   │   │   └── Interfaces/
    │   │   └── Infrastructure/
    │   │       ├── Models/        ← EF scaffolded models
    │   │       ├── Repositories/
    │   │       └── AppDbContext.cs
    │   └── Modules/
    │       ├── Inventory/
    │       │   ├── Application/
    │       │   ├── Domain/
    │       │   ├── Infrastructure/
    │       │   └── Presentation/
    │       └── Sales/
    │           ├── Application/
    │           ├── Domain/
    │           ├── Infrastructure/
    │           ├── Presentation/
    │           └── SubModules/
    │               └── PdV/
    └── Program.cs
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [dotnet-ef CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/henrrycoronado/InventarySystem.git
cd InventarySystem
```

### 2. Configure the database environment

```bash
cd DataBase
cp .env.example .env
```

Edit `DataBase/.env` with your values:

```env
POSTGRES_USER=admin_saas_HC
POSTGRES_PASSWORD=YOUR_PASSWORD
POSTGRES_DB=InventarySystemDB
```

### 3. Start the database

```bash
docker compose up -d
```

Verify schemas were created:

```bash
docker exec -it inventarysystem_db psql -U admin_saas_HC -d InventarySystemDB -c "\dn"
```

You should see: `shared`, `inventory`, `sales`.

### 4. Configure the API environment

```bash
cd ../InventarySystem.Api
cp .env.example .env
```

Edit `InventarySystem.Api/.env` with your values:

```env
DB_CONNECTION_STRING=Host=localhost;Port=5432;Database=InventarySystemDB;Username=admin_saas_HC;Password=YOUR_PASSWORD
```

### 5. Run the API

```bash
dotnet run
```

Swagger available at: `http://localhost:PORT/swagger`

---

## Clean Restart Protocol

Use when you want to reset all data and start fresh:

```bash
cd DataBase
docker compose down -v
docker compose up -d
cd ../InventarySystem.Api
dotnet run
```

---

## API Structure

All endpoints follow this response format:

```json
{
  "success": true,
  "data": {},
  "message": null
}
```

### Core Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/companies` | List all active companies |
| POST | `/api/companies` | Create company |
| GET | `/api/companies/{id}/warehouses` | List warehouses by company |
| POST | `/api/companies/{id}/warehouses` | Create warehouse |
| GET | `/api/global-categories` | List global categories |
| POST | `/api/global-categories` | Create global category |
| GET | `/api/global-products` | List global products |
| POST | `/api/global-products` | Create global product |

### Inventory Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/companies/{id}/products` | List company products |
| POST | `/api/companies/{id}/products` | Create company product |
| GET | `/api/products/{id}/skus` | List SKUs by product |
| POST | `/api/products/{id}/skus` | Create SKU |
| GET | `/api/warehouses/{id}/stocks` | List stock by warehouse |
| POST | `/api/warehouses/{id}/stocks` | Create stock entry |
| POST | `/api/companies/{id}/movements/incoming` | Register incoming movement |
| POST | `/api/companies/{id}/movements/outgoing` | Register outgoing movement |
| GET | `/api/warehouses/{id}/kardex/sku/{skuId}` | Get kardex by SKU |

### Sales Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/companies/{id}/customers` | List customers |
| POST | `/api/companies/{id}/customers` | Create customer |
| GET | `/api/companies/{id}/sellers` | List sellers |
| POST | `/api/companies/{id}/sellers` | Create seller |
| GET | `/api/companies/{id}/sales` | List sales |
| POST | `/api/companies/{id}/sales` | Create sale |
| POST | `/api/companies/{id}/sales/{id}/confirm` | Confirm sale |
| POST | `/api/companies/{id}/sales/{id}/cancel` | Cancel sale |
| POST | `/api/sales/{id}/receipt` | Generate receipt |

### PdV Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/companies/{id}/pdv/tables` | List tables |
| POST | `/api/companies/{id}/pdv/tables` | Create table |
| GET | `/api/companies/{id}/pdv/waiters` | List waiters |
| POST | `/api/companies/{id}/pdv/waiters` | Create waiter |
| GET | `/api/companies/{id}/pdv/menus` | List menus |
| POST | `/api/companies/{id}/pdv/menus` | Create menu |
| GET | `/api/pdv/menus/{id}/items` | List menu items |
| POST | `/api/pdv/menus/{id}/items` | Add menu item |
| GET | `/api/companies/{id}/pdv/orders` | List orders |
| POST | `/api/companies/{id}/pdv/orders` | Open order |
| POST | `/api/companies/{id}/pdv/orders/{id}/items` | Add item to order |
| PATCH | `/api/companies/{id}/pdv/orders/{id}/items/{detailId}/status` | Update item status |
| POST | `/api/companies/{id}/pdv/orders/{id}/checkout` | Checkout order |

---

## Branching Strategy

GitFlow strict:

- `main` — stable code, ready for deploy
- `develop` — main integration branch
- `feature/[name]` — one branch per feature, merged to `develop` via Pull Request

No direct push to `main` or `develop`.

---

## Commit Convention

This repository uses **Conventional Commits**:

```
type(module): description in lowercase
```

| Type | When to use |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `chore` | Configuration, dependencies |
| `refactor` | Restructure without behavior change |
| `docs` | Documentation |
| `test` | Tests |

**Valid example:** `feat(inventory): add stock reservation on pdv order`

---

## Business Rules

- **Available stock** is always `quantity - reserved_quantity`. Never use `quantity` directly for validations.
- **Soft delete only** — no physical DELETE. Everything is deactivated with `is_active = false`.
- **Kardex is immutable** — records are never updated or deleted, only inserted.
- **Multi-company** — every query is scoped by `company_id`.
- **Global products** — every `company_product` must reference a `global_product`. If it doesn't exist in the global catalog, it must be created there first.

---

## Known Technical Debt

- `SaleService` and `PdvOrderService` currently inject `IStockRepository` directly from the Inventory module. This should be refactored to use `IInventoryService` defined in `Core/Contracts` to respect module boundaries.
