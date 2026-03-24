using System.ComponentModel.DataAnnotations;

namespace TaskManager_backend.DTOs;

public class TaskCreateDto
{
    [Required(ErrorMessage = "El título es requerido")]
    [MinLength(3, ErrorMessage = "El título debe tener al menos 3 caracteres")]
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "La prioridad es requerida")]
    public required string Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
}

public class TaskUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
