using Application.Contracts;
using Application.DTOs.AuthDtos;
using Application.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;

namespace WebApi.Controllers;


[Route("api/account")]
[ApiController]
public class AccountController(
    IAuthService authService) : ControllerBase
{
    // Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var response = await authService.LoginAsync(model);
        return response.ToActionResult();
    }
    
    // Logout
    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutRequest)
    {
        var result = await authService.LogoutAsync(logoutRequest.RefreshToken);
        return result.ToActionResult();
    }
    
    // Refresh
    [HttpPost("refresh-token")]
    [InputValidationFilter<RefreshRequestDto>]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto refreshRequest)
    {
        var response = await authService.RefreshTokenAsync(refreshRequest.AccessToken, refreshRequest.RefreshToken);
        return response.ToActionResult();
    }
    
    [HttpPost("add-role")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public async Task<IActionResult> AddRole([FromBody] AddRoleRequest request)
    {
        await authService.AddToRoleAsync(request.Role, null, request.Email);
        return Ok();
    }
}
