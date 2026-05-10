# Mejoras Buildertrend - Roadmap y Estado

## Mejoras Base (Completadas)
- [x] CRUD de cotizaciones y productos
- [x] Cálculo en tiempo real y validación robusta
- [x] Folio único y lógica master-detail
- [x] UI/UX avanzada y accesibilidad
- [x] Adjuntos (QuoteAttachment)
- [x] Historial de cambios (AuditLog)
- [x] Visualización de historial en detalles
- [x] Seguimiento de mejoras (este archivo)
- [x] Comentarios internos (QuoteComment)
- [x] Estados personalizados (EstadoCotizacion)
- [x] Notificaciones por correo
- [x] Plantillas configurables
- [x] Entidad de eventos de calendario
- [x] Dashboard avanzado con filtros/exportar
- [x] Buscador global inteligente (Aplicado)

## Mejoras Avanzadas (Implementadas - Mayo 2026)

### #31 - Sistema de Versiones de Cotizaciones
**Estado:** ✅ COMPLETADO
- Entidad `QuoteVersion` con snapshots de cotización
- Permite restaurar versiones anteriores
- Registro de cambios por versión
- UI en details para ver y restaurar versiones

### #32 - Firma Electrónica
**Estado:** ✅ COMPLETADO
- Entidad `QuoteSignature` para capturar firmas
- Hash de autenticación y timestamp
- Visualización de datos de firma en detalles
- Integración con flujo de aprobación

### #34 - Permisos Granulares por Rol
**Estado:** ✅ COMPLETADO
- Entidad `Permission` con categorías
- Tabla `RolePermissions` para mapeo rol-permiso
- Servicio `IPermissionService` para gestión
- Atributo `PermissionAttribute` para control de acceso
- Permisos soportados:
  - quote:view (Ver cotizaciones)
  - quote:create (Crear cotizaciones)
  - quote:edit (Editar cotizaciones)
  - quote:delete (Eliminar cotizaciones)
  - quote:approve (Aprobar cotizaciones)
  - quote:view-costs (Ver costos y márgenes)
  - quote:sign (Firmar cotizaciones)
  - quote:restore-version (Restaurar versiones)
- Rol `Empleado` preconfigurado con permisos base
- UI de gestión de permisos por rol

## Bug Fixes (Mayo 2026)
- ✅ Cálculo incorrecto de Sales Tax con números separados por miles (5.80 → 5,800.00)
- ✅ Warning de HTTPS redirect en entorno Development

## Validación
Cada mejora ha sido implementada con:
- Migración EF Core aplicada
- Base de datos actualizada
- Pruebas funcionales completadas
- Compatibilidad con flujos existentes
