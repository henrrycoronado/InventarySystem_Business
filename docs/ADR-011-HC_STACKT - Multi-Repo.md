# ADR-011-HC_STACKT — Multi-Repo: Frontend y Backend en Repositorios Separados

## Status
**Status:** Accepted

## Context
InventarySystem tiene frontend (React) y backend (.NET). Se requiere decidir si mantenerlos en un único repo (monorepo) o en repos separados (multi-repo). Cada enfoque afecta workflow, CI/CD, versionado y coordinación del equipo.

## Decision
Utilizaremos **Multi-Repo: repositorio separado para frontend (InventarySystem_Web) y backend (InventarySystem_Business)**. Cada repo tiene su propio ciclo de vida, versioning y despliegue independiente.

## Alternatives Considered
- **Monorepo (frontend + backend en mismo repo):** Facilita cambios coordinados pero complica CI/CD, builds lentos, y dependencias entrelazadas.
- **Monorepo con workspaces (Nx/pnpm):** Mejor que monorepo simple pero complejidad operacional; ideal para empresas grandes.
- **Repos anidados (submódulos Git):** Peor de ambos mundos: complejidad de multi-repo sin beneficios de monorepo.

## Consequences
**Positivas:**
- Despliegue independiente: cambios en frontend no requieren redeploy backend
- Ciclos de release separados: frontend puede iterar rápido sin esperar backend
- Equipos independientes: frontend y backend sin bloqueos mutuos
- Historiales de commit limpios: cada repo enfocado en su dominio
- Herramientas optimizadas: cada stack con sus mejores prácticas

**Negativas:**
- Coordinación más difícil: cambios API requieren sincronización manual
- Versionado de contrato: frontend y backend deben estar en versiones compatibles
- CI/CD más complejo: pipelines separados que deben estar sincronizados
- Integración local: developer necesita ambos repos clonados
- Cambios en contrato API pueden romper integración si no hay comunicación