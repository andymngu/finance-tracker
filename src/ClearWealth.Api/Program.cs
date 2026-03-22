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

builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:3000")
          .AllowAnyHeader()
          .AllowAnyMethod()));

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(opt => opt.TokenValidationParameters = new()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = "clearwealth",
//        ValidAudience = "clearwealth",
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
//    });

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();  // ← this must be here
app.Run();