// src/ClearWealth.Api/Program.cs
using ClearWealth.Application.Interfaces;
using ClearWealth.Application.Services;
using ClearWealth.Application.Stubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountRepository, StubAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, StubTransactionRepository>();
builder.Services.AddScoped<IPlaidClient, StubPlaidClient>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TransactionService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();  // ← this must be here
app.Run();