// src/ClearWealth.Api/Controllers/AccountsController.cs
using ClearWealth.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClearWealth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    // Hardcoded user for now — replaced by [Authorize] + JWT claims in Phase 1
    private static readonly Guid _stubUserId =
        Guid.Parse("00000000-0000-0000-0000-000000000001");

    private readonly AccountService _svc;
    public AccountsController(AccountService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAccounts() =>
        Ok(await _svc.GetAccountsAsync(_stubUserId));

    [HttpGet("net-worth")]
    public async Task<IActionResult> GetNetWorth() =>
        Ok(await _svc.GetNetWorthAsync(_stubUserId));
}