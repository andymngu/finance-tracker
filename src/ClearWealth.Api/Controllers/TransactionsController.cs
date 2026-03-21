// src/ClearWealth.Api/Controllers/TransactionsController.cs
using ClearWealth.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClearWealth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private static readonly Guid _stubUserId =
        Guid.Parse("00000000-0000-0000-0000-000000000001");

    private readonly TransactionService _svc;
    public TransactionsController(TransactionService svc) => _svc = svc;

    [HttpGet("account/{accountId:guid}")]
    public async Task<IActionResult> GetByAccount(Guid accountId) =>
        Ok(await _svc.GetRecentAsync(accountId));

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _svc.GetAllForUserAsync(_stubUserId));

    [HttpGet("spending-by-category")]
    public async Task<IActionResult> GetSpendingByCategory() =>
        Ok(await _svc.GetSpendingByCategory(_stubUserId));

    [HttpGet("cash-flow")]
    public async Task<IActionResult> GetCashFlow() =>
        Ok(await _svc.GetMonthlyCashFlowAsync(_stubUserId));
}