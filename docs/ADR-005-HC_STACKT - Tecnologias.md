# ADR-005-HC_STACKT — Tecnologias: .NET REST API + React SPA + JWT/OAuth

## Status
**Status:** Accepted

## Context
InventarySystem necesita comunicación entre frontend (React) y backend (.NET). Se requiere elegir protocolo de comunicación, autenticación y autorización. REST es estándar de facto para SaaS.

## Decision
Utilizaremos **REST API en .NET** sin autenticación momentaneamente en el desarrollo de la beta. Para casos futuros de integración, soportaremos **JWT (JSON Web Tokens)** para una primera instancia de autentificacion.

## Alternatives Considered
- **GraphQL:** Mayor precisión de queries pero complejidad operacional innecesaria para nuestros endpoints.
- **gRPC:** Excelente para microservicios pero overkill para monolito SPA.
- **Session-based (cookies):** Menos flexible para múltiples clientes (móvil, web); JWT es stateless.

## Consequences
**Positivas:**
- REST es estándar conocido y fácil de entender
- JWT stateless: no requiere sesiones en servidor
- Escalable: fácil agregar clientes (móvil, third-party)
- Autenticación sin estado simplifica clusters
- OAuth permite integraciones futuras

**Negativas:**
- JWT puede ser más grande que cookies
- Revocación de tokens requiere lista negra
- Overhead de validación JWT en cada request
- Requiere HTTPS para seguridad