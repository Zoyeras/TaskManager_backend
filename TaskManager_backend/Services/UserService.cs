using Microsoft.EntityFrameworkCore;
using TaskManager_backend.Data;
using TaskManager_backend.DTOs;
using TaskManager_backend.Models;
using TaskManager_backend.Services;

namespace TaskManager_backend.Services;

public interface IUserService
{
    Task<ApiResponse<UserResponseDto>> RegisterAsync(UserRegisterDto registerDto);
    Task<ApiResponse<string>> LoginAsync(UserLoginDto loginDto);
    Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int userId);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public UserService(AppDbContext context, IPasswordService passwordService, IJwtService jwtService)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<UserResponseDto>> RegisterAsync(UserRegisterDto registerDto)
    {
        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return ApiResponse<UserResponseDto>.ErrorResult("Las contraseñas no coinciden");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
        if (existingUser != null)
        {
            return ApiResponse<UserResponseDto>.ErrorResult("El email ya está registrado");
        }

        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = _passwordService.HashPassword(registerDto.Password),
            Role = "user"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userResponse = new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<UserResponseDto>.SuccessResult(userResponse, "Usuario registrado exitosamente");
    }

    public async Task<ApiResponse<string>> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return ApiResponse<string>.ErrorResult("Credenciales inválidas");
        }

        if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return ApiResponse<string>.ErrorResult("Credenciales inválidas");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
        return ApiResponse<string>.SuccessResult(token, "Login exitoso");
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return ApiResponse<UserResponseDto>.ErrorResult("Usuario no encontrado");
        }

        var userResponse = new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<UserResponseDto>.SuccessResult(userResponse);
    }
}
