# TaskManager Backend

API REST para gestion de tareas con autenticacion JWT, autorizacion por rol y persistencia en PostgreSQL.

## Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core + Npgsql
- PostgreSQL
- JWT (Bearer)

## Funcionalidades

- Registro e inicio de sesion de usuarios
- Emision de token JWT
- CRUD de tareas protegido por autenticacion
- Autorizacion por rol (`admin` y `user`) en operaciones de tareas
- Manejo global de excepciones
- Validacion de modelos con filtro global
- Conversion personalizada de `DateTime` en JSON

## Requisitos

- .NET 10 SDK
- PostgreSQL disponible en `localhost:5432` (o cadena de conexion equivalente)

## Configuracion

La app usa estas claves en configuracion:

- `ConnectionStrings:DefaultConnection`
- `JwtSettings:SecretKey`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`
- `JwtSettings:ExpirationHours`

En `TaskManager_backend/appsettings.json` ya existe un ejemplo funcional para desarrollo local.

Para produccion, configura estos valores por variables de entorno:

- `ConnectionStrings__DefaultConnection`
- `JwtSettings__SecretKey`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`
- `JwtSettings__ExpirationHours`

## Base de datos

Puedes iniciar PostgreSQL con Docker Compose desde la raiz del workspace:

```bash
docker compose up -d db
```

Luego aplica migraciones desde la carpeta del proyecto (`TaskManager_backend/TaskManager_backend`):

```bash
dotnet ef database update
```

## Ejecutar la API

Desde la raiz del workspace:

```bash
dotnet run --project TaskManager_backend/TaskManager_backend.csproj
```

Segun `Properties/launchSettings.json`, en desarrollo se usa:

- `http://localhost:5027`
- `https://localhost:7294`

## Autenticacion y autorizacion

- `POST /api/auth/register` y `POST /api/auth/login` son publicos.
- Todos los endpoints de `tasks` requieren token Bearer (`[Authorize]`).
- El rol se lee desde el claim `ClaimTypes.Role`.
- `admin` puede consultar y operar tareas sin filtrar por usuario.
- `user` solo opera sus propias tareas.

## Endpoints principales

| Metodo | Endpoint | Auth | Descripcion |
| --- | --- | --- | --- |
| POST | `/api/auth/register` | No | Registro de usuario |
| POST | `/api/auth/login` | No | Login y entrega de JWT |
| GET | `/api/tasks` | Si | Lista tareas (admin: todas, user: propias) |
| GET | `/api/tasks/{id}` | Si | Obtiene una tarea por id |
| POST | `/api/tasks` | Si | Crea una tarea para el usuario autenticado |
| PUT | `/api/tasks/{id}` | Si | Actualiza una tarea |
| DELETE | `/api/tasks/{id}` | Si | Elimina una tarea |

## Estructura del proyecto

```text
TaskManager_backend/
├── Controllers/
├── Converters/
├── Data/
├── DTOs/
├── Filters/
├── Middleware/
├── Migrations/
├── Models/
├── Services/
├── Program.cs
└── TaskManager_backend.csproj
```

## Notas de desarrollo

- CORS permite origenes `http://localhost:3000` y `http://localhost:5173`.
- OpenAPI se expone solo en entorno `Development`.

