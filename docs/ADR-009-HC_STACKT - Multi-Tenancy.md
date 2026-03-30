# ADR-009-HC_STACKT — Multi-Tenancy por company_id (Aislamiento de Datos)

## Status
**Status:** Accepted

## Context
InventarySystem_Business es multi-tenant: múltiples empresas usan la plataforma. Se requiere garantizar que los datos de una empresa nunca sean visibles para otra. El aislamiento debe ser a nivel de aplicación, no solo de base de datos.

## Decision
Implementaremos **filtrado automático por company_id** en todos los queries. El company_id vendrá del JWT del usuario autenticado. Un middleware global filtrará datos antes de ser servidos. Cada entidad tiene un company_id FK que no es nulo.

## Alternatives Considered
- **Row-Level Security (RLS) de PostgreSQL:** Más seguro a nivel BD pero requiere políticas complejas; difícil debuggear.
- **Múltiples bases de datos por tenant:** Mayor aislamiento pero costo operacional alto; backups complejos.
- **Sin aislamiento:** Catastrófico: data leak entre empresas; brechas de seguridad.

## Consequences
**Positivas:**
- Seguridad de datos garantizada: empresa A nunca ve datos de empresa B
- Escalabilidad: agregar tenants no requiere infraestructura nueva
- Costo-efectivo: una BD para todos
- Debugging más fácil que RLS (SQL puro visible)
- Compliance y privacidad de datos

**Negativas:**
- Riesgo crítico: un bug en filtro expone datos de otros tenants
- Requiere disciplina: TODOS los queries deben filtrar company_id
- Difícil debuggear si se olvida filtro (datos ausentes sin error)
- Performance: un tenant grande ralentiza queries de otros