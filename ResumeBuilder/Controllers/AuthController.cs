﻿using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.DTOs;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (result == null) return BadRequest("User already exists.");
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null) return Unauthorized();
        return Ok(result);
    }
}
