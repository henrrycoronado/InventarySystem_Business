# ADR-006-HC_STACKT — Manejo de Imágenes: Object Storage + CDN + Presigned URLs

## Status
**Status:** Proposed

## Context
InventarySystem_Business maneja imágenes de productos. Almacenarlas en el servidor local no escala; requiere storage externo. Se necesita decidir cómo servir imágenes de forma rápida, segura y escalable.

## Decision
Utilizaremos **Object Storage (S3/Azure Blob)** para almacenar imágenes, **CDN (CloudFront/Azure CDN)** para distribución geográfica rápida, y **Presigned URLs** para acceso temporal y seguro sin exponer credenciales.

## Alternatives Considered
- **Almacenar en servidor local:** No escala; requiere más almacenamiento, backups manuales, no hay CDN.
- **Base de datos (BLOB):** Ralentiza BD; difícil escalar; innecesario para archivos.
- **URLs públicas permanentes:** Fácil pero menos seguro; no hay control de acceso.

## Consequences
**Positivas:**
- Escalabilidad ilimitada: agregar imágenes no afecta servidor
- CDN acelera descarga en cualquier geolocalización
- Presigned URLs: acceso temporal sin credenciales expuestas
- Bajo costo: Object Storage es barato
- Backup automático del proveedor

**Negativas:**
- Latencia de creación de presigned URL
- Costo adicional de transferencia de datos
- Complejidad en setup y mantenimiento de acceso
- Dependencia del proveedor cloud