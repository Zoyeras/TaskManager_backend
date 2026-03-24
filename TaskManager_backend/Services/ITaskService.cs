using TaskManager_backend.DTOs;

namespace TaskManager_backend.Services;

/// <summary>
/// Interfaz que define las operaciones para la gestión de tareas,
/// considerando el rol del usuario (admin o usuario normal).
/// </summary>
public interface ITaskService
{
    // Crear tarea (solo el usuario propietario puede crearla, no cambia con admin)
    Task<ApiResponse<TaskResponseDto>> CreateTaskAsync(TaskCreateDto taskDto, int userId);

    /// <summary>
    /// Obtiene tareas.
    /// Si isAdmin es true, devuelve todas las tareas de todos los usuarios.
    /// Si es false, solo las del userId indicado.
    /// </summary>
    Task<ApiResponse<List<TaskResponseDto>>> GetUserTasksAsync(int userId, bool isAdmin);

    /// <summary>
    /// Obtiene una tarea por su ID.
    /// Si isAdmin es true, busca la tarea sin importar el userId.
    /// Si es false, busca la tarea que pertenezca al userId.
    /// </summary>
    Task<ApiResponse<TaskResponseDto>> GetTaskByIdAsync(int taskId, int userId, bool isAdmin);

    /// <summary>
    /// Actualiza una tarea.
    /// Si isAdmin es true, puede actualizar cualquier tarea (sin verificar userId).
    /// Si es false, solo puede actualizar si la tarea pertenece al userId.
    /// </summary>
    Task<ApiResponse<TaskResponseDto>> UpdateTaskAsync(int taskId, TaskUpdateDto taskDto, int userId, bool isAdmin);

    /// <summary>
    /// Elimina una tarea.
    /// Si isAdmin es true, puede eliminar cualquier tarea.
    /// Si es false, solo puede eliminar si la tarea pertenece al userId.
    /// </summary>
    Task<ApiResponse<bool>> DeleteTaskAsync(int taskId, int userId, bool isAdmin);
}