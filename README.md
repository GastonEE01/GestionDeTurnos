#Sistema de Gestión de Turnos

Aplicación Fullstack (E2E) diseñada para la reserva y administración de turnos dinámicos en tiempo real, adaptable a múltiples rubros (peluquerías, centros de estética, canchas deportivas, etc.). El foco principal del proyecto es resolver problemas complejos de arquitectura, sincronización de datos distribuidos y alta concurrencia en entornos web.

Funcionalidades:

- **Reserva Eficiente de Turnos:** Interfaz de calendario dinámico que calcula y renderiza los bloques de horarios disponibles basados en la duración de cada servicio y la agenda del local.
- **Sincronización en Tiempo Real (WebSockets):** Integración con SignalR para reflejar instantáneamente el bloqueo o reserva de turnos a todos los usuarios conectados, evitando vistas desactualizadas.
- **Control de Alta Concurrencia:** Mecanismo de Concurrencia Optimista en el backend para prevenir reservas duplicadas cuando dos o más usuarios intentan adquirir el mismo slot horario simultáneamente.
- **Optimización de Búsquedas (Debouncing):** Filtrado inteligente de locales y servicios en el frontend reduciendo la carga del servidor mediante la demora controlada de peticiones HTTP.
- **Gestión Basada en Roles:** Autenticación segura para usuarios (visualización de historial y cancelación de turnos propios) y administradores (configuración de locales, alta de servicios y control de agenda).


## Tecnologias

- **Frontend:** React, TypeScript, Vite, Zustand (Estado Global del Hub y UI), React Query (Cacheo y Sincronización de Datos), Tailwind CSS.
- **Backend:** .NET (Web API), Entity Framework Core, SignalR (WebSockets).
- **Base de Datos:** PostgreSQL (alojada en Neon.tech).