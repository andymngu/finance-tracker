using ClearWealth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearWealth.Api.Extensions;

namespace ClearWealth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly AccountService _svc;
    public AccountsController(AccountService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts() =>
        Ok(await _svc.GetAccountsAsync(User.GetUserId()));

    [HttpGet("net-worth")]
    public async Task<IActionResult> GetNetWorth() =>
        Ok(await _svc.GetNetWorthAsync(User.GetUserId()));
}