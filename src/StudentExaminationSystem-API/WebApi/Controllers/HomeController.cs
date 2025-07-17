using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Helpers.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController(
    IDashboardService dashboardService
    ) : ControllerBase
{
    [HttpGet("Dashboard")]
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await dashboardService.GetAdminDashboardAppAsync();
        return dashboard.ToActionResult();
    }
}
