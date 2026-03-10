namespace TaskManager_backend.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "user";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Relacion con Tareas
    public List<TaskItem> Tasks { get; set; } = new();
}