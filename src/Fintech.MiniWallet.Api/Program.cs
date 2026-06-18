using Fintech.MiniWallet.Api.Exceptions;
using Fintech.MiniWallet.Application.Accounts.Commands.Deposit;
using Fintech.MiniWallet.Domain.Interfaces;
using Fintech.MiniWallet.Infrastructure.Persistence;
using Fintech.MiniWallet.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DepositHandler).Assembly));
builder.Services.AddDbContext<MiniWalletDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
      options.SwaggerEndpoint("/openapi/v1.json", "MiniWallet API v1"));
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();

app.Run();
