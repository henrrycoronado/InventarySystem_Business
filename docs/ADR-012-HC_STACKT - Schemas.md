# ADR-012-HC_STACKT — Estrategia de Schemas en PostgreSQL por Módulo

## Status
**Status:** Accepted

## Context
InventarySystem_Business tiene múltiples módulos (Inventario, Ventas, PDV). La base de datos PostgreSQL necesita organización clara para aislar tablas por dominio, facilitar mantenimiento y evitar conflictos de nombres. Se requiere decidir estrategia de schemas.

## Decision
Utilizaremos **schemas PostgreSQL por módulo**: `inventory`, `sales`, `pdv`, `shared`. Cada schema agrupa tablas relacionadas a su dominio. Tablas compartidas (users, companies) irán en `shared`. El aislamiento es lógico, no de datos (todos pertenecen a mismo company_id multi-tenant).

## Alternatives Considered
- **Todas las tablas en schema public:** Simple inicialmente pero imposible navegar conforme crece; riesgo de conflictos de nombres.
- **Un schema por tenant:** Mayor aislamiento pero BD explota en tamaño; backups complejos; costo alto.
- **Prefijos en nombres (inventory_products, sales_orders):** Menos limpio que schemas; difícil de navegar.

## Consequences
**Positivas:**
- Organización clara: tablas agrupadas por dominio de negocio
- Fácil navegar y mantener: schema refleja arquitectura de módulos
- Escalabilidad: agregar módulo es agregar schema
- Permisos granulares: puedes restringir acceso a schemas por rol
- Refleja arquitectura en código: schemas = carpetas en proyecto

**Negativas:**
- Queries más verbosas: schema.tabla en lugar de tabla
- Requiere disciplina: developer debe saber qué schema usar
- Migraciones más complejas: deben especificar schema
- Documentación necesaria: equipo debe entender la estrategia