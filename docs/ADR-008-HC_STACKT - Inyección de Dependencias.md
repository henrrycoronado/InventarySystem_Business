# ADR-008-HC_STACKT — Inyección de Dependencias en Constructores

## Status
**Status:** Accepted

## Context
InventarySystem_Business tiene múltiples servicios, repositorios y dependencias. Sin inyección de dependencias, el código estaría acoplado, difícil de testear y mantener. .NET proporciona DI nativo en .NET Core.

## Decision
Utilizaremos **Inyección de Dependencias (DI) en constructores** registrando servicios en el contenedor de .NET Core (Startup.cs/Program.cs). Cada clase recibirá sus dependencias por constructor, no las creará internamente.

## Alternatives Considered
- **Service Locator Pattern:** Anti-pattern; oculta dependencias; difícil testear.
- **New keyword directo:** Acoplamiento fuerte; imposible mockear en tests; código rígido.
- **Property Injection:** Menos seguro; dependencias opcionales no obvias.

## Consequences
**Positivas:**
- Código testeable: fácil inyectar mocks en unit tests
- Bajo acoplamiento: cambiar implementación sin tocar consumidores
- Centralizando configuración: Program.cs es fuente única de verdad
- Escalable: agregar nuevos servicios es registrar en DI
- Ciclo de vida controlado (Transient/Scoped/Singleton)

**Negativas:**
- Overhead mínimo de reflexión en startup
- Requiere entendimiento de DI patterns
- Posibles circular dependencies si no se diseña bien
- Debugging más indirecto (saltar a través de interfaces)