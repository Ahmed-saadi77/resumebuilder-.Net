using ResumeBuilder.DTOs;


public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(AuthRequestDto dto);
    Task<AuthResponseDto?> LoginAsync(AuthRequestDto dto);
}
