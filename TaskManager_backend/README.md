# TaskManager Backend

API RESTful para gestión de tareas con autenticación JWT.

## Tecnologías

- .NET 10.0
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- ASP.NET Core Web API

## Endpoints

### Autenticación
- `POST /api/auth/register` - Registrar usuario
- `POST /api/auth/login` - Iniciar sesión

### Tareas
- `GET /api/tasks` - Obtener tareas del usuario
- `GET /api/tasks/{id}` - Obtener tarea específica
- `POST /api/tasks` - Crear nueva tarea
- `PUT /api/tasks/{id}` - Actualizar tarea
- `DELETE /api/tasks/{id}` - Eliminar tarea

## Configuración

1. Configurar conexión a PostgreSQL en `appsettings.json`
2. Ejecutar migraciones: `dotnet ef database update`
3. Iniciar servidor: `dotnet run`

## Variables de Entorno

Para producción, usar variables de entorno:
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__SecretKey`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`
