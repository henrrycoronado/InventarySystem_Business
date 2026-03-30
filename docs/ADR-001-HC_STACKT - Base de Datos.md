# ADR-001-HC_STACKT — Base de Datos: PostgreSQL Multi-Tenant para SaaS

## Status
**Status:** Accepted

## Context
InventarySystem_Business es un backend SaaS que servirá múltiples clientes (tenants). Se requiere una base de datos que permita aislamiento de datos por tenant, escalabilidad horizontal y que sea cost-effective. Se necesita elegir entre arquitectura de BD multi-tenant.

## Decision
Utilizaremos **PostgreSQL con enfoque multi-tenant a nivel schema o fila** (company_id). Cada cliente (empresa) tendrá sus datos aislados por un company_id. La base de datos es compartida pero los datos están lógicamente separados.

## Alternatives Considered
- **Base de datos separada por tenant:** Mayor aislamiento y seguridad pero costo operacional alto; complica backups y mantenimiento.
- **MongoDB (NoSQL):** Flexible pero menos adecuado para transacciones financieras (ventas, inventario).
- **SQL Server:** Similar a PostgreSQL pero mayor costo de licencia; PostgreSQL es open-source.

## Consequences
**Positivas:**
- Costo-efectivo: una sola BD para todos los tenants
- Escalabilidad: agregar nuevos clientes sin provisionar BD nueva
- Backups centralizados simplificados
- Fácil migrar datos entre ambientes
- Replicación y high availability más simple

**Negativas:**
- Riesgo de data leak si fallan filtros company_id
- Queries más complejas (siempre filtrar por company_id)
- Capacidad compartida: un tenant con mucho tráfico afecta otros
- Recuperación por tenant más difícil