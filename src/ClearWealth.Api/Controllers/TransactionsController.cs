// src/ClearWealth.Api/Controllers/TransactionsController.cs
using ClearWealth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClearWealth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _svc;
    public TransactionsController(TransactionService svc) => _svc = svc;

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new InvalidOperationException("User ID not found in token"));

    [HttpGet("account/{accountId:guid}")]
    public async Task<IActionResult> GetByAccount(Guid accountId) =>
        Ok(await _svc.GetRecentAsync(accountId));

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _svc.GetAllForUserAsync(GetUserId()));

    [HttpGet("spending-by-category")]
    public async Task<IActionResult> GetSpendingByCategory() =>
        Ok(await _svc.GetSpendingByCategory(GetUserId()));

    [HttpGet("cash-flow")]
    public async Task<IActionResult> GetCashFlow() =>
        Ok(await _svc.GetMonthlyCashFlowAsync(GetUserId()));
}