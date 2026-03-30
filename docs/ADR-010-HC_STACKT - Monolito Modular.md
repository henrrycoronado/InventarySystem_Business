# ADR-010-HC_STACKT — Monolito Modular en Proyecto Único vs Multi-Proyecto

## Status
**Status:** Accepted

## Context
InventarySystem_Business es un monolito modular. Se requiere decidir si organizar módulos (Inventario, Ventas, PDV) dentro de un único proyecto .NET o separarlos en múltiples proyectos acoplados en una solución (.sln). Cada enfoque tiene trade-offs de complejidad, mantenimiento y escalabilidad.

## Decision
Utilizaremos **un único proyecto .NET con módulos internos** organizados por carpetas (Inventory/, Sales/, POS/). No crearemos proyectos separados por módulo. Esto mantiene simplicidad de despliegue sin sacrificar modularidad lógica.

## Alternatives Considered
- **Multi-proyecto (.sln con Inventory.csproj, Sales.csproj, POS.csproj):** Mayor aislamiento pero complejidad de referencias circulares, versionado, y despliegue fragmentado.
- **Microservicios con repos separados:** Máxima independencia pero overhead operacional excesivo para equipo pequeño.
- **Monolito sin estructura (todo mezclado):** Simple inicialmente pero imposible mantener conforme crece.

## Consequences
**Positivas:**
- Despliegue simple: un único .dll/.exe
- Transacciones ACID entre módulos sin overhead
- Menos complejidad de build y versionado
- Fácil compartir utilities entre módulos
- Ideal para equipos pequeños/medianos

**Negativas:**
- Require disciplina para mantener módulos desacoplados
- Escalabilidad limitada: todo crece junto
- Difícil escalar un módulo específico independientemente
- Un bug en un módulo puede afectar toda la app