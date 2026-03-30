# ADR-002-HC_STACKT — Infraestructura: App + DB Gestionada

## Status
**Status:** Proposed

## Context
InventarySystem_Business necesita infraestructura escalable, confiable y cost-effective. Se requiere decidir entre gestionar servidores manualmente o usar servicios gestionados en la nube. Se necesita considerar BD, almacenamiento de imágenes, distribución de contenido y API hosting.

## Decision
Utilizaremos una **arquitectura cloud con servicios gestionados**: App (.NET) en contenedores (Docker/Kubernetes o App Service), PostgreSQL gestionada (RDS/Azure Database).

## Alternatives Considered
- **Servidores dedicados on-premise:** Máximo control pero alto costo operacional, mantenimiento complejo y escalabilidad limitada.
- **IaaS puro (VMs):** Mayor control pero responsabilidad de patching, backups y mantenimiento manual.
- **Monolito en un solo servidor:** Falla en escalabilidad y disponibilidad; punto único de fallo.

## Consequences
**Positivas:**
- Estabilidad asegurada.
- Alta disponibilidad y disaster recovery integrado
- Backups automáticos y georeplicación
- Mejor seguridad: servidores mantenidos por proveedor

**Negativas:**
- Vendor lock-in: cambiar proveedor es costoso
- Costos variables pueden ser impredecibles