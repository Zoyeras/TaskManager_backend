using TaskManager_backend.Models;

namespace TaskManager_backend.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
}