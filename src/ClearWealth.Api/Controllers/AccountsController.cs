using ClearWealth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClearWealth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly AccountService _svc;
    public AccountsController(AccountService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        return Ok(await _svc.GetAccountsAsync(Guid.Parse(userId)));
    }

    [HttpGet("net-worth")]
    public async Task<IActionResult> GetNetWorth()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        return Ok(await _svc.GetNetWorthAsync(Guid.Parse(userId)));
    }
}