# ADR-003-HC_STACKT — Arquitectura Monolito Modular

## Status
**Status:** Accepted

## Context
InventarySystem_Business gestiona inventario, ventas, PDV y otros módulos. Se requiere decidir entre monolito vs microservicios. Un monolito bien estructurado es más simple de desplegar y mantener para equipos pequeños.

## Decision
Utilizaremos una **arquitectura de monolito modular + Clean Architecture** en .NET. Cada dominio (inventario, ventas, PDV) será un módulo independiente dentro del mismo proceso, con separación clara de responsabilidades pero desplegados juntos, cada uno tendra internamente sus propias capas de la arquitectura clean.

## Alternatives Considered
- **Microservicios:** Mayor escalabilidad independiente pero complejidad operacional, problemas de consistencia distribuida y mayor latencia.
- **Monolito sin estructura (ball of mud):** Desarrollo rápido inicialmente pero mantenimiento imposible a medida que crece.
- **Arquitectura en capas simple:** Menos modular; difícil escalar dominios específicos.

## Consequences
**Positivas:**
- Despliegue simple: un único artefacto (.dll/.exe)
- Transacciones ACID garantizadas entre módulos
- Debugging más fácil que microservicios
- Mejor rendimiento: sin latencia de red entre módulos
- Ideal para equipos pequeños

**Negativas:**
- Escalabilidad limitada: todo crece junto
- Un error en un módulo puede derribar toda la app
- Difícil usar diferentes tecnologías por módulo
- Despliegues más riesgosos (cambio en un módulo afecta todos)