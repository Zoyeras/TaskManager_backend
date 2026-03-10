namespace TaskManager_backend.Models;

public class TaskItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Pendiente";
    public required string Priority { get; set; }
    public DateTime? DueDate { get; set; }
    
    // Relacion con el usuario
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}