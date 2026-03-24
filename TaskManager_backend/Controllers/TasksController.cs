using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager_backend.DTOs;
using TaskManager_backend.Services;

namespace TaskManager_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // Obtiene el ID del usuario desde el token JWT
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    // Obtiene el rol del usuario desde el token JWT
    private string GetCurrentUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        return roleClaim != null ? roleClaim.Value : "user";
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto taskDto)
    {
        var userId = GetCurrentUserId();
        // La creación de tareas no cambia: siempre se asigna al usuario actual
        var result = await _taskService.CreateTaskAsync(taskDto, userId);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();
        var isAdmin = role == "admin";

        // Pasamos isAdmin para que el servicio decida si filtrar por usuario o no
        var result = await _taskService.GetUserTasksAsync(userId, isAdmin);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();
        var isAdmin = role == "admin";

        var result = await _taskService.GetTaskByIdAsync(id, userId, isAdmin);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();
        var isAdmin = role == "admin";

        var result = await _taskService.UpdateTaskAsync(id, taskDto, userId, isAdmin);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();
        var isAdmin = role == "admin";

        var result = await _taskService.DeleteTaskAsync(id, userId, isAdmin);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}