// src/ClearWealth.Api/Controllers/TransactionsController.cs
using ClearWealth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearWealth.Api.Extensions;

namespace ClearWealth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _svc;
    public TransactionsController(TransactionService svc)
    {
        _svc = svc;
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<IActionResult> GetByAccount(Guid accountId) =>
        Ok(await _svc.GetRecentAsync(accountId));

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _svc.GetAllForUserAsync(User.GetUserId()));

    [HttpGet("spending-by-category")]
    public async Task<IActionResult> GetSpendingByCategory() =>
        Ok(await _svc.GetSpendingByCategory(User.GetUserId()));

    [HttpGet("cash-flow")]
    public async Task<IActionResult> GetCashFlow() =>
        Ok(await _svc.GetMonthlyCashFlowAsync(User.GetUserId()));
}