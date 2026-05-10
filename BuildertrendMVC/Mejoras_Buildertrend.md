# 🚀 Mejoras al Sistema Buildertrend

> Este documento lleva el control de las mejoras propuestas, su descripción y estado de implementación.

---

| Nº | Mejora | Descripción breve | Estado |
|:--:|:-----------------------------------------------|:-------------------------------------------------------------------------------|:--------:|
| 1  | 📊 **Dashboard con métricas y gráficos**        | Tarjetas con KPIs y gráficos de ventas/cotizaciones usando Chart.js            | ✅ Aplicado |
| 2  | 🔎 **Búsqueda y filtrado avanzado**             | Buscador y filtros en tablas de productos y cotizaciones                       | ✅ Aplicado |
| 3  | 🔔 **Notificaciones y alertas**                 | Alertas de éxito/error y notificaciones de tareas/cotizaciones                 | ✅ Aplicado |
| 4  | 📄 **Exportar a PDF/Excel**                     | Exportar cotizaciones o productos a PDF/Excel                                  | ✅ Aplicado |
| 5  | 👥 **Gestión de usuarios y roles**               | Autenticación y roles para controlar acceso                                    | ✅ Aplicado |
| 6  | 📱 **Responsive y modo oscuro**                  | Mejoras móviles y botón para modo oscuro                                       | ✅ Aplicado |
| 7  | 🎨 **Personalización visual**                    | Cambiar logo, colores o nombre de empresa desde configuración                  | ✅ Aplicado |
| 8  | 🕓 **Historial y auditoría**                     | Historial de cambios en cotizaciones y productos                               | ✅ Aplicado |
| 9  | 👁️ **Botón “Ver Detalle” en cotizaciones**      | Permite ver la cotización completa en una página aparte, mostrando todas las partidas y totales. | ✅ Aplicado |
| 10 | 🧩 **Búsqueda y filtros avanzados en cotizaciones** | Filtrar por folio, fecha, cliente, monto total o texto en partidas.         | ✅ Aplicado |
| 11 | ↕️ **Ordenar columnas en cotizaciones**         | Permite ordenar por folio, total, fecha, etc.                                  | ✅ Aplicado |
| 12 | 📤 **Exportar cotización a PDF/Excel**           | Opción para exportar la cotización seleccionada o todas a PDF/Excel.           | ✅ Aplicado |
| 13 | 📑 **Paginación en cotizaciones**                | Agregar paginación o scroll infinito si hay muchas cotizaciones.               | ✅ Aplicado |
| 14 | 🗓️ **Columna de fecha en cotizaciones**         | Mostrar la fecha de creación de la cotización.                                 | ✅ Aplicado |
| 15 | 🏷️ **Columna de estado en cotizaciones**        | Agregar un estado (ej: “Borrador”, “Enviada”, “Aprobada”, “Cancelada”).        | ✅ Aplicado |
| 16 | ⚡ **Acciones rápidas en cotizaciones**          | Botones para duplicar, eliminar, enviar por correo o imprimir la cotización.    | ✅ Aplicado |
| 17 | 🏅 **Resumen visual en cotizaciones**            | Mostrar badges de cantidad de partidas, monto total o alertas si falta información. | ✅ Aplicado |
| 18 | 🔒 **Login como pantalla inicial** | Al ejecutar el sistema, la primera pantalla siempre es el login, incluso si el usuario no está autenticado. | ✅ Aplicado |
| 19 | 📎 **Adjuntar archivos a cotizaciones** | Permitir subir y asociar documentos (planos, imágenes, PDFs) a cada cotización. | ✅ Aplicado |
| 20 | 📝 **Historial de cambios por campo** | Mostrar un historial detallado de cambios por campo en cada cotización (quién, cuándo y qué cambió). | ✅ Aplicado |
| 21 | 💬 **Comentarios y notas internas** | Agregar un sistema de comentarios o notas internas por cotización, visibles solo para usuarios internos. | ✅ Aplicado |
| 22 | 🔄 **Estados personalizados y flujos de aprobación** | Permitir definir estados personalizados y flujos de aprobación (ej: “En revisión”, “Aprobación Gerente”, “Rechazada”). | ✅ Aplicado |
| 23 | 📧 **Notificaciones automáticas por correo** | Enviar correos automáticos al cliente o usuarios internos según cambios de estado o acciones clave. | ✅ Aplicado |
| 24 | 🖨️ **Plantillas de cotización configurables** | Permitir definir y seleccionar plantillas de impresión/exportación para cotizaciones. | ✅ Aplicado |
| 25 | 📅 **Integración con calendario** | Agendar recordatorios o fechas clave (vencimiento, seguimiento) y visualizarlas en un calendario. | ✅ Aplicado |
| 26 | 📈 **Dashboard avanzado con filtros y exportación** | Dashboard con métricas filtrables por usuario, periodo, estado, producto, etc., y exportación de reportes. | ✅ Aplicado |
| 27 | 🔍 **Buscador global inteligente** | Búsqueda rápida por cualquier campo, incluyendo texto en partidas, comentarios y archivos adjuntos. | ✅ Aplicado |
| 28 | 🔗 **API REST para integración externa** | Exponer endpoints seguros para integración con otros sistemas (ERP, CRM, apps móviles). | ✅ Aplicado |
| 29 | 👤 **Gestión de clientes y contactos** | Relacionar cotizaciones con clientes/contactos, historial de cotizaciones por cliente, y ficha de cliente. | ✅ Aplicado |
| 30 | 💱 **Soporte multimoneda y multi-idioma** | Permitir cotizar en diferentes monedas y mostrar la interfaz en varios idiomas. | ✅ Aplicado |
| 31 | 🗂️ **Control de versiones de cotización** | Permitir guardar versiones previas de una cotización y restaurar versiones anteriores. | ✅ Aplicado |
| 32 | ✍️ **Firma electrónica de cotizaciones** | Integrar firma digital para aprobación de cotizaciones por parte del cliente. | ✅ Aplicado |
| 33 | 🌐 **Internacionalización total de la interfaz** | Todas las vistas principales usan recursos .resx y cambian idioma según preferencia del usuario. | 🚧 En progreso (Clientes, Productos, Cotizaciones, Usuarios listos) |
| 34 | 🛡️ **Permisos avanzados por rol** | Definir permisos granulares: quién puede editar, aprobar, eliminar, ver costos, etc. | ✅ Aplicado |
| 35 | 🔐 **Acceso restringido sin login** | Ninguna pantalla es accesible si el usuario no ha iniciado sesión; todo el sistema requiere autenticación. | ✅ Aplicado |

---

### 🟢 Leyenda de Estado

- ✅ **Aplicado**: Mejora implementada y funcional
- ⏳ **Pendiente**: Mejora aún no implementada

---

## 🐛 Bug Fixes y Correcciones Recientes

| Descripción | Problema | Solución | Fecha | Estado |
|:------------|:---------|:---------|:------|:-------|
| **Sales Tax Amount mal calculado** | Cálculo mostraba $5.80 en lugar de $5,800.00 para totales como 80,000 | Implementar parseo robusto de moneda que soporte ambos formatos (en-US: 80,000.00 y es-XX: 80.000,00) en `parseLocalizedCurrency()` | 09/05/2026 | ✅ Corregido |
| **Cache de JavaScript en navegador** | Cambios en JS no se reflejaban porque el navegador cachea archivos | Agregar `asp-append-version="true"` a script tags en Create.cshtml y Edit.cshtml | 09/05/2026 | ✅ Aplicado |
| **HTTPS redirect warning en Development** | Middleware lanzaba error "Failed to determine https port" en modo desarrollo | Condicionar `app.UseHttpsRedirection()` con `if (!app.Environment.IsDevelopment())` | 08/05/2026 | ✅ Corregido |

---

## � Gestión de Usuarios y Roles (Mejorada 09/05/2026)

### Vistas Actualizadas

#### **Página Index** - Listado de Usuarios  
- 📊 Tabla profesional con info completa: Email, Username, Roles asignados, Moneda, Idioma
- 🎨 Badges coloreados para roles y preferencias
- 🔒 Solo Admin puede editar usuarios
- ⚡ UI mejorada con iconos y estilos Bootstrap 5

#### **Página Edit** - Editar Usuario y Asignar Roles
- 📋 Formulario moderno con tarjetas organizadas
- 🔐 Sección dedicada para asignación de roles con descripciones
- ⚙️ Panel lateral con preferencias (Moneda, Idioma)
- 📊 Resumen en tiempo real de roles asignados (se actualiza al hacer check/uncheck)
- 💬 Descripciones de roles para facilitar administración

### Rutas de Acceso
- **Index**: `/Users` (menú navegación disponible para Admin)
- **Edit**: `/Users/Edit/{id}` (solo Admin puede acceder)

### Permisos Requeridos
- Solo usuarios con rol **Admin** pueden acceder a la gestión de usuarios
- El UsersController está protegido con `[Authorize]`
- La acción Edit valida `User.IsInRole("Admin")`

---

## �📋 Características Implementadas en Sesión Actual (08-09/05/2026)

### #31 - Sistema de Versiones de Cotizaciones ✅
- **Tabla QuoteVersion**: Almacena snapshots de cotizaciones con `VersionNumber`, `CreatedAt`, `Snapshot` (JSON), `RestoredFromVersionId`
- **Funcionalidad de Restore**: Endpoint `/Quote/RestoreVersion/{quoteId}/{versionId}` restaura cotización a versión anterior
- **UI en Details**: Listado de versiones con botón "Restaurar" para cada versión

### #32 - Firma Electrónica ✅
- **Tabla QuoteSignature**: Almacena firma digital con `SignatureHash`, `SignatureDate`, `ClientName`, `ClientEmail`
- **Captura de firma**: Formulario en Details.cshtml con canvas para dibujar firma
- **Validación**: Hash SHA256 para autenticar integridad de firma

### #34 - Permisos Granulares por Rol ✅
- **8 Permisos implementados**:
  - `quote:view` - Ver cotizaciones
  - `quote:create` - Crear cotizaciones
  - `quote:edit` - Editar cotizaciones
  - `quote:delete` - Eliminar cotizaciones
  - `quote:approve` - Aprobar cotizaciones
  - `quote:view-costs` - Ver costos y márgenes
  - `quote:sign` - Firmar cotizaciones
  - `quote:restore-version` - Restaurar versiones anteriores

- **PermissionService**: Queries RolePermissions para resolver permisos de usuario
- **PermissionAttribute**: Custom authorization attribute para validar permisos en controllers/actions
- **Seeding**: PermissionSeeder asigna todos los permisos al rol "Empleado" en startup
- **Compatibilidad**: Fallback para usuarios autenticados sin rol asignado explícitamente

---

> Las mejoras se irán marcando como **Aplicado** conforme se desarrollen.

---

## CRUD Completo de Usuarios - Sistema Mejorado (09/05/2026)

### Acciones Implementadas

**CREATE** - Crear nuevo usuario
- Ruta: POST /Users/Create  
- Validación de email único
- Confirmación de contraseña
- Asignación de múltiples roles
- Configuración de preferencias (Moneda, Idioma)

**READ** - Listar usuarios
- Ruta: GET /Users
- Tabla con Email, Username, Roles, Moneda, Idioma
- Botón "Crear Usuario" para Admin
- Botones Editar/Eliminar para Admin

**UPDATE** - Editar usuario
- Ruta: GET/POST /Users/Edit/{id}
- Modificar asignación de roles
- Actualizar preferencias
- Resumen en tiempo real de roles

**DELETE** - Eliminar usuario
- Ruta: POST /Users/Delete/{id}
- Confirmación antes de eliminar
- Protección: no permite auto-eliminación
- Mensajes de confirmación

### Cinco Roles con Permisos Granulares

1. **Admin**: Todos los 8 permisos
2. **Empleado**: 5 permisos (crear, editar, ver, ver-costos, firmar)
3. **Gerente**: 3 permisos (ver, aprobar, ver-costos)
4. **Cliente**: 2 permisos (ver, firmar)
5. **Contador**: 2 permisos (ver, ver-costos)

### Archivos Modificados

- Controllers/UsersController.cs: Acciones Create, Delete y métodos auxiliares
- Views/Users/Create.cshtml: Nueva vista para crear usuarios
- Views/Users/Index.cshtml: Actualizada con botones Crear y Eliminar
- Models/UserEditViewModel.cs: Propiedades Password y ConfirmPassword agregadas
- Services/PermissionSeeder.cs: Semilla mejorada con 5 roles y permisos
- Views/Users/Edit.cshtml: Ya existía, funciona con sistema actualizado

### Validaciones Implementadas

- Email único en el sistema
- Contraseña requerida (6+ caracteres)
- Confirmación de contraseña debe coincidir
- No permitir eliminar usuario actual
- Solo Admin puede CRUD usuarios
- Validación de antiforgery token

### Status: ✅ Completado y Compilado

Proyecto compila exitosamente. CRUD funcional con roles y permisos integrados.

---

## 🔐 Usuario Admin - Creación Automática (09/05/2026)

### Usuario Admin Automático

Al iniciar la aplicación, se crea automáticamente un usuario Admin:

| Propiedad | Valor |
|:----------|:------|
| **Email** | `admin@buildertrend.com` |
| **Contraseña** | `Admin123!@` |
| **Rol** | Admin (todos los 8 permisos) |
| **Moneda** | USD |
| **Idioma** | Español (es) |

### Archivos Implementados

- **Servicio**: [Services/AdminSeeder.cs](Services/AdminSeeder.cs) - Crea usuario Admin en primera ejecución
- **Integración**: [Program.cs](Program.cs) - Llama a `AdminSeeder.SeedAdminAsync()` al startup
- **Automático**: No requiere intervención manual, se ejecuta al iniciar `dotnet run`

### Seguridad Recomendada

Después de primer login con credenciales por defecto:
1. Cambiar contraseña de admin@buildertrend.com
2. Crear nuevos usuarios con rol Admin si es necesario
3. Considerar deshabilitar el seeding automático después del primer deployment

### Steps para Usar

1. **Ejecutar la app**:
   ```bash
   dotnet run
   ```

2. **Login como Admin**:
   - Email: `admin@buildertrend.com`
   - Contraseña: `Admin123!@`

3. **Acceder a Gestión de Usuarios**:
   - URL: `http://localhost:5077/Users`
   - Aquí puedes crear más usuarios con diferentes roles

### Status: ✅ Implementado y Compilado
