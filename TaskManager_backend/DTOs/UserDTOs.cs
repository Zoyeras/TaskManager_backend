using System.ComponentModel.DataAnnotations;

namespace TaskManager_backend.DTOs;

public class UserRegisterDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = "Confirmar contraseña es requerido")]
    public required string ConfirmPassword { get; set; }
}

public class UserLoginDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    public required string Password { get; set; }
}

public class UserResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
