# ADR-004-HC_STACKT — Estructura de Carpetas: Domain-Driven + Feature-Based

## Status
**Status:** Accepted

## Context
InventarySystem_Business crece con múltiples dominios (Inventario, Ventas, PDV, Autenticación). Sin una estructura clara de carpetas, el código se vuelve difícil de navegar, mantener y escalar. Se necesita organización que refleje la lógica de negocio.

## Decision
Utilizaremos una **estructura de carpetas híbrida: Domain-Driven Design + Feature-Based**. Carpetas por dominio (Inventory/, Sales/, POS/), dentro de cada una: Entities, ValueObjects, Services, Repositories, DTOs, Controllers. Cada dominio es independiente pero comunica con otros a través de servicios bien definidos.

## Alternatives Considered
- **Estructura Layer-Based (Controllers/, Services/, Repositories/):** Difícil navegar cuando proyecto crece; mezcla dominios no relacionados.
- **Flat Structure:** Todo en raíz; imposible mantener conforme crece.
- **DDD puro con Bounded Contexts separados:** Más modular pero complejidad innecesaria para monolito.

## Consequences
**Positivas:**
- Código organizado por dominio de negocio
- Fácil encontrar código relacionado
- Escalable: agregar nuevo dominio es add una carpeta
- Facilita onboarding de desarrolladores
- Refleja la lógica de negocio

**Negativas:**
- Requiere disciplina para mantener límites entre dominios
- Posible duplicación de código si no se centralizan utilities
- Requiere que equipo entienda DDD