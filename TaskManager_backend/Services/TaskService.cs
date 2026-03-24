using Microsoft.EntityFrameworkCore;
using TaskManager_backend.Data;
using TaskManager_backend.DTOs;
using TaskManager_backend.Models;

namespace TaskManager_backend.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<TaskResponseDto>> CreateTaskAsync(TaskCreateDto taskDto, int userId)
    {
        var task = new TaskItem
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Priority = taskDto.Priority,
            DueDate = taskDto.DueDate,
            Status = "Pendiente",
            UserId = userId
            // CreatedAt se asigna automáticamente por el modelo (DateTime.UtcNow)
            // UpdatedAt queda null inicialmente
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var taskResponse = new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            UserId = task.UserId,
            CreatedAt = task.CreatedAt,       // Usar el valor persistido
            UpdatedAt = task.UpdatedAt
        };

        return ApiResponse<TaskResponseDto>.SuccessResult(taskResponse, "Tarea creada exitosamente");
    }

    public async Task<ApiResponse<List<TaskResponseDto>>> GetUserTasksAsync(int userId, bool isAdmin)
    {
        var query = _context.Tasks.AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(t => t.UserId == userId);
        }

        var tasks = await query
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                UserId = t.UserId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return ApiResponse<List<TaskResponseDto>>.SuccessResult(tasks);
    }

    public async Task<ApiResponse<TaskResponseDto>> GetTaskByIdAsync(int taskId, int userId, bool isAdmin)
    {
        var query = _context.Tasks.AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(t => t.Id == taskId && t.UserId == userId);
        }
        else
        {
            query = query.Where(t => t.Id == taskId);
        }

        var task = await query.FirstOrDefaultAsync();

        if (task == null)
        {
            return ApiResponse<TaskResponseDto>.ErrorResult("Tarea no encontrada");
        }

        var taskResponse = new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            UserId = task.UserId,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };

        return ApiResponse<TaskResponseDto>.SuccessResult(taskResponse);
    }

    public async Task<ApiResponse<TaskResponseDto>> UpdateTaskAsync(int taskId, TaskUpdateDto taskDto, int userId, bool isAdmin)
    {
        var query = _context.Tasks.AsQueryable();
        if (!isAdmin)
        {
            query = query.Where(t => t.Id == taskId && t.UserId == userId);
        }
        else
        {
            query = query.Where(t => t.Id == taskId);
        }

        var task = await query.FirstOrDefaultAsync();

        if (task == null)
        {
            return ApiResponse<TaskResponseDto>.ErrorResult("Tarea no encontrada");
        }

        // Actualizar campos si vienen en el DTO
        if (taskDto.Title != null) task.Title = taskDto.Title;
        if (taskDto.Description != null) task.Description = taskDto.Description;
        if (taskDto.Status != null) task.Status = taskDto.Status;
        if (taskDto.Priority != null) task.Priority = taskDto.Priority;
        if (taskDto.DueDate != null) task.DueDate = taskDto.DueDate;

        // Actualizar timestamp
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var taskResponse = new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            UserId = task.UserId,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };

        return ApiResponse<TaskResponseDto>.SuccessResult(taskResponse, "Tarea actualizada exitosamente");
    }

    public async Task<ApiResponse<bool>> DeleteTaskAsync(int taskId, int userId, bool isAdmin)
    {
        var query = _context.Tasks.AsQueryable();
        if (!isAdmin)
        {
            query = query.Where(t => t.Id == taskId && t.UserId == userId);
        }
        else
        {
            query = query.Where(t => t.Id == taskId);
        }

        var task = await query.FirstOrDefaultAsync();

        if (task == null)
        {
            return ApiResponse<bool>.ErrorResult("Tarea no encontrada");
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResult(true, "Tarea eliminada exitosamente");
    }
}