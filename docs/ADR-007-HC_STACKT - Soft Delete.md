# ADR-007-HC_STACKT — Soft Delete Pattern (IsActive) para Auditoría

## Status
**Status:** Accepted

## Context
InventarySystem_Business maneja datos críticos (productos, ventas, clientes). En lugar de eliminar físicamente registros, se requiere mantener historial completo para auditoría, recuperación y compliance. Se necesita un patrón de eliminación lógica.

## Decision
Implementaremos **Soft Delete Pattern**: cada tabla tendrá una columna `IsActive` (boolean, default true). Al "eliminar" un registro, se marca `IsActive = false` en lugar de borrarlo. Los queries filtran por `IsActive = true` por defecto.

## Alternatives Considered
- **Hard Delete (DROP):** Pierdes historial; imposible auditar o recuperar; incumple compliance.
- **Tabla de auditoría separada:** Más complejo; requiere triggers; duplica datos.
- **Versionado completo (temporal tables):** SQL Server feature; PostgreSQL requiere extensiones; overkill para nuestro caso.

## Consequences
**Positivas:**
- Auditoría completa: ves qué fue eliminado y cuándo
- Recuperación fácil: set `IsActive = true` nuevamente
- Compliance: cumple regulaciones que requieren historial
- Sin riesgo de data loss por errores de eliminación
- Queries simples con filtro `IsActive`

**Negativas:**
- Base de datos crece: nunca libera espacio de registros "eliminados"
- Queries deben filtrar `IsActive` siempre (fácil olvidar)
- Performance: más registros en tablas (índices más lentos)
- Requiere disciplina: todos los queries deben incluir filtro