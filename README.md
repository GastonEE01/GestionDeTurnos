## Gestion de turnos
Aplicación Fullstack (E2E) diseñada para la reserva y administración de turnos dinámicos en tiempo real, adaptable a múltiples rubros (peluquerías, centros de estética, canchas deportivas, etc.). El foco principal del proyecto es resolver problemas complejos de arquitectura, sincronización de datos distribuidos y alta concurrencia en entornos web.

## 🔗 Enlaces del Proyecto
DEMO Frontend (Vercel): https://gestion-de-turnos-zeta.vercel.app/

API Backend (Azure): https://gestiondeturnos-bkg7cpgtcucah7hy.brazilsouth-01.azurewebsites.net/index.html

## Funcionalidades:
*Autenticación y Roles:** Sistema de inicio de sesión y registro diferenciando permisos para Clientes (reservar y ver sus turnos) y Locales (administrar su comercio).
*Gestión de Locales y Servicios:** Los administradores de cada local pueden dar de alta sus servicios especificando el nombre y la duración en minutos de cada uno.
*Configuración de Horarios:** Definición de los días de atención, horarios de apertura/cierre y días de descanso de cada comercio.
*Reserva de Turnos en Línea:** El cliente selecciona un servicio, visualiza los días y horarios hábiles del local, elige una fecha y el sistema calcula automáticamente los bloques libres disponibles.
*Panel de Administración de Turnos:** Vista para los locales donde pueden consultar el listado de reservas recibidas (con detalle de cliente, servicio, fecha y hora exacta) y cancelar turnos si es necesario.
*Validaciones de Negocio:** El backend valida que no se puedan tomar turnos fuera de horario, en días que el local permanece cerrado o duplicados en el mismo slot.

## Tecnologías Utilizadas

### Frontend
* React (Vite)
* React Router DOM
* JavaScript (ES6+)
* CSS Vanilla

### Backend 
* .NET (ASP.NET Core Web API)
* Entity Framework Core
* JWT Authentication
* Arquitectura por capas / Use Cases

### Base de Datos
* PostgreSQL
* neontech

### Seguridad
* Protección de endpoints con JWT.
* Variables sensibles gestionadas mediante Environment Variables en Azure.
* Exclusión de secretos mediante .gitignore.

### Deploy
* Vercel (Frontend)
* Azure App Service (Backend)
