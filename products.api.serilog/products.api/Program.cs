using FluentValidation;
using Microsoft.EntityFrameworkCore;
using products.api.Extensions;
using products.api.Middleware;
using products.api.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logConfg) => logConfg.ReadFrom.Configuration(context.Configuration));
// Add services to the container.

builder.Services.AddControllers()

    ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);


var app = builder.Build();
app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ApplyMigrations();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
