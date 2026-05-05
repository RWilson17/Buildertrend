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
| 30 | 💱 **Soporte multimoneda y multi-idioma** | Permitir cotizar en diferentes monedas y mostrar la interfaz en varios idiomas. | ⏳ Pendiente |
| 31 | 🗂️ **Control de versiones de cotización** | Permitir guardar versiones previas de una cotización y restaurar versiones anteriores. | ⏳ Pendiente |
| 32 | ✍️ **Firma electrónica de cotizaciones** | Integrar firma digital para aprobación de cotizaciones por parte del cliente. | ⏳ Pendiente |
| 33 | 🛡️ **Permisos avanzados por rol** | Definir permisos granulares: quién puede editar, aprobar, eliminar, ver costos, etc. | ⏳ Pendiente |

---

### 🟢 Leyenda de Estado

- ✅ **Aplicado**: Mejora implementada y funcional
- ⏳ **Pendiente**: Mejora aún no implementada

---

> Las mejoras se irán marcando como **Aplicado** conforme se desarrollen.
