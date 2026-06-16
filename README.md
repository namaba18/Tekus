# Tekus

Prueba técnica: gestión de proveedores (suppliers) y sus servicios asociados.

El proyecto está dividido en dos aplicaciones independientes:

- **`backend/`** — API REST en .NET 10 (Clean Architecture: Domain / Application / Infrastructure / API).
- **`frontend/`** — SPA en Angular 21 que consume la API.

---

## Arquitectura

### Backend (`backend/`)

Solución `Tekus.slnx` con 4 proyectos principales (Domain, Application, Infrastructure, API) más `UnitTests` e `IntegrationTests`, siguiendo Clean Architecture: `Domain` no depende de nada, `Application` solo depende de `Domain`, `Infrastructure` implementa los contratos de `Application`, y `API` ensambla todo.

**Patrones usados:**

- **CQRS + MediatR**: cada caso de uso es un `Command`/`Query` con su `Handler` en `Application/Features/...`.
- **Repository pattern**: `Application` solo conoce interfaces (`ISupplierRepository`, `IServiceRepository`, `IUnitOfWork`), implementadas en `Infrastructure` con EF Core. `Application` no depende de EF Core ni de ningún detalle de persistencia.
- **Domain Events**: las entidades heredan de `BaseEntity`, que permite registrar eventos de dominio (`AddDomainEvent`). `AppDbContext.SaveChangesAsync` los despacha automáticamente después de persistir, a través de `IEventDispatcher`. Ejemplo: al crear un `Service` se dispara `ServiceCreatedEvent`, manejado por `ServiceCreatedEmailNotificationHandler`, que envía un correo de notificación.
- **Pipeline behaviors de MediatR**: `ValidationBehavior` ejecuta los validadores de FluentValidation antes de cada handler.
- **Autenticación JWT** con un único usuario por defecto (no hay gestión de usuarios): las credenciales están en `appsettings.json` bajo `DefaultUser`.

**Entidades principales:**

- `Supplier` (NIT, Name, WebPage, Email) — un proveedor puede tener muchos `Service`.
- `Service` (Name, HourlyRate) — pertenece a un único `Supplier`.

**Endpoints principales:**

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Autenticación, devuelve un JWT | No |
| GET | `/api/suppliers` | Lista todos los proveedores | Sí |
| GET | `/api/suppliers/{id}` | Detalle de un proveedor | Sí |
| GET | `/api/suppliers/{id}/services` | Servicios de un proveedor | Sí |
| POST | `/api/services` | Crea un servicio para un proveedor (dispara notificación por correo) | Sí |

### Frontend (`frontend/Tekus.Web/`)

Angular 21 (standalone components, signals, control flow `@if`/`@for`).

- **Autenticación**: `AuthService` guarda el JWT en `localStorage`; `authInterceptor` lo agrega a cada request (`Authorization: Bearer ...`); `authGuard` protege las rutas privadas y redirige a `/login` si no hay sesión.
- **Rutas**:
  - `/login` — formulario de login.
  - `/` — lista de proveedores (página inicial tras autenticarse). El nombre de cada proveedor enlaza a su página de servicios.
  - `/suppliers/:supplierId/services` — servicios del proveedor, con botón para crear uno nuevo.

---

## Requisitos previos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 20+ y npm
- SQL Server (local o accesible) — o ajustar la cadena de conexión para usar otro proveedor compatible con EF Core
- [Angular CLI](https://angular.dev/tools/cli) (opcional, se puede usar `npx ng`)
- `dotnet-ef` (para migraciones): `dotnet tool install --global dotnet-ef`

---

## Cómo ejecutar el backend

```bash
cd backend

# 1. Restaurar y compilar
dotnet build Tekus.slnx

# 2. Configurar la cadena de conexión en src/Tekus.API/appsettings.json
#    ("ConnectionStrings:Database") y, si aplica, las credenciales SMTP
#    en "Email" y el destinatario de notificaciones en "Notifications".

# 3. Aplicar las migraciones (crea la base de datos y el seed de proveedores)
dotnet ef database update --project src/Tekus.Infrastructure --startup-project src/Tekus.API

# 4. Levantar la API
dotnet run --project src/Tekus.API
```

La API queda disponible en `http://localhost:5259` (ver `src/Tekus.API/Properties/launchSettings.json`).

### Usuario por defecto

No hay registro ni administración de usuarios. Las credenciales están fijas en `appsettings.json` (`DefaultUser`):

- **Usuario:** `admin`
- **Contraseña:** `Admin123!`

### Pruebas

```bash
cd backend
dotnet test Tekus.slnx
```

---

## Cómo ejecutar el frontend

```bash
cd frontend/Tekus.Web

npm install
npm start            # equivalente a `ng serve`
```

La SPA queda disponible en `http://localhost:4200` y espera que la API esté corriendo en `http://localhost:5259` (configurado en `src/app/core/api.config.ts` y habilitado vía CORS en el backend para ese origen).

Otros comandos útiles:

```bash
npm run build     # build de producción
npm test          # tests con Vitest
```

---

## Configuración del envío de correo

Cada vez que se crea un `Service` para un proveedor, se dispara el evento de dominio `ServiceCreatedEvent`, que es manejado por `ServiceCreatedEmailNotificationHandler` (en `Tekus.Application`) y envía un correo a través de `SmtpEmailSender` (en `Tekus.Infrastructure`), usando `System.Net.Mail.SmtpClient`.

La configuración vive en `src/Tekus.API/appsettings.json`, en dos secciones:

```json
"Email": {
  "Host": "smtp.example.com",
  "Port": 587,
  "UseSsl": true,
  "Username": "",
  "Password": "",
  "FromAddress": "no-reply@tekus.com",
  "FromName": "Tekus"
},
"Notifications": {
  "NewServiceRecipientEmail": "compras@tekus.com"
}
```

| Clave | Descripción |
|---|---|
| `Email:Host` | Servidor SMTP (ej. `smtp.gmail.com`, `smtp.office365.com`, `smtp.sendgrid.net`). |
| `Email:Port` | Puerto SMTP (587 para STARTTLS, 465 para SSL implícito). |
| `Email:UseSsl` | Si la conexión debe usar SSL/TLS. |
| `Email:Username` / `Email:Password` | Credenciales de autenticación SMTP. |
| `Email:FromAddress` / `Email:FromName` | Remitente que verá el destinatario del correo. |
| `Notifications:NewServiceRecipientEmail` | Destinatario que recibe el aviso de "nuevo servicio habilitado". Es la "preferencia del sistema" mencionada en el requerimiento: al no existir administración de usuarios/preferencias, se configura directamente aquí. |

**Para probarlo localmente** se puede usar cualquier proveedor SMTP real (Gmail con contraseña de aplicación, Outlook, SendGrid, Mailtrap, etc.) completando `Host`, `Port`, `Username` y `Password`. Por defecto el `Host` es un placeholder (`smtp.example.com`) y el envío fallará: esto **no** rompe la creación del servicio, ya que el handler captura cualquier error de envío y solo lo registra en el log (`ILogger`), sin revertir la transacción.

Si `Notifications:NewServiceRecipientEmail` queda vacío, el envío se omite (con un warning en el log) en lugar de fallar.

---

## Notas adicionales

- **JWT**: la clave de firma (`Jwt:Key` en `appsettings.json`) es un valor de ejemplo. En un entorno real debe reemplazarse por un secreto fuerte gestionado fuera del control de versiones (variables de entorno, secret manager, etc.).
- **Seed de datos**: la migración `SeedSuppliers` inserta dos proveedores de ejemplo al aplicar `dotnet ef database update`.
