﻿namespace Harmony.Application.Models.DTOs.AuthDTOs;

public class LoginRequestDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
