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

### Catalog Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/catalogs/movement-statuses` | List movement statuses |
| GET | `/api/catalogs/movement-types` | List movement types |
| GET | `/api/catalogs/sale-statuses` | List sale statuses |
| GET | `/api/catalogs/pdv-order-statuses` | List PdV order statuses |
| GET | `/api/catalogs/pdv-item-statuses` | List PdV item statuses |

### Attributes & Batches Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/companies/{id}/attributes` | List company attributes |
| POST | `/api/companies/{id}/attributes` | Create attribute |
| DELETE | `/api/companies/{id}/attributes/{id}` | Deactivate attribute |
| GET | `/api/skus/{skuId}/attributes` | List SKU attribute values |
| POST | `/api/skus/{skuId}/attributes` | Add attribute value to SKU |
| DELETE | `/api/skus/{skuId}/attributes/{id}` | Deactivate attribute value |
| GET | `/api/skus/{skuId}/batches` | List batches by SKU |
| POST | `/api/skus/{skuId}/batches` | Create batch |
| DELETE | `/api/skus/{skuId}/batches/{id}` | Deactivate batch |

### Station Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/companies/{id}/pdv/stations` | List stations |
| POST | `/api/companies/{id}/pdv/stations` | Create station |
| DELETE | `/api/companies/{id}/pdv/stations/{id}` | Deactivate station |
| GET | `/api/pdv/stations/{stationId}/categories` | List station categories |
| POST | `/api/pdv/stations/{stationId}/categories` | Assign category to station |
| DELETE | `/api/pdv/stations/{stationId}/categories/{id}` | Remove category from station |

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

- `PdvOrderService.AddItemAsync` uses `warehouseId = 0` as placeholder when reserving stock for PdV items. The correct warehouseId should be passed from the order context. Pending refinement.

---

## Integration Guide

### Catalog Endpoints

Use these to populate dropdowns dynamically. Never hardcode IDs.

**GET** `/api/catalogs/movement-statuses`
**GET** `/api/catalogs/movement-types`
**GET** `/api/catalogs/sale-statuses`
**GET** `/api/catalogs/pdv-order-statuses`
**GET** `/api/catalogs/pdv-item-statuses`

All return:
```json
[{ "id": 1, "code": "DRAFT", "name": "Borrador" }]
```

---

### Attributes & SKU Variants

Attributes define the dimensions of variation for a product (e.g. Talla, Color).
SKU attribute values are the specific values for each SKU.

**GET** `/api/companies/{companyId}/attributes`
**POST** `/api/companies/{companyId}/attributes`
```json
{ "name": "Talla" }
```

**GET** `/api/skus/{skuId}/attributes`
**POST** `/api/skus/{skuId}/attributes`
```json
{ "attributeId": 1, "value": "42" }
```

---

### Batches

Batches allow tracking stock by lot number and expiration date.
`batchId` is optional in all stock, movement and sale operations.

**GET** `/api/skus/{skuId}/batches`
**POST** `/api/skus/{skuId}/batches`
```json
{
  "batchNumber": "LOT-2026-001",
  "manufactureDate": "2026-01-01",
  "expirationDate": "2027-01-01"
}
// manufactureDate and expirationDate are optional
```

---

### Stations (PdV)

Stations represent physical preparation points (kitchen, bar, cashier).
Each station handles specific global categories.
Menu items are assigned to a station — when an order detail is created,
it inherits the station from the menu item automatically.

**GET** `/api/companies/{companyId}/pdv/stations`
**POST** `/api/companies/{companyId}/pdv/stations`
```json
{ "name": "Cocina" }
```

**GET** `/api/pdv/stations/{stationId}/categories`
**POST** `/api/pdv/stations/{stationId}/categories`
```json
{ "globalCategoryId": 9 }
// Links a global category to this station
// Use GET /api/global-categories to get available categories
```

### Station flow for PdV kitchen display

```
1. GET /api/companies/{id}/pdv/stations  ← load stations
2. GET /api/pdv/stations/{id}/categories ← know which categories each station handles
3. Poll GET /api/companies/{id}/pdv/orders ← filter by open orders
4. Filter order details by stationId     ← show only relevant items
5. PATCH order detail status             ← update as items are prepared
```
