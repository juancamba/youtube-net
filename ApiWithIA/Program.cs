using ApiWithIA.Data;
using ApiWithIA.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<AiService>();
builder.Services.AddScoped<TicketService>();
// 🔥 CORS para React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
//app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
builder.Services.AddHttpClient<AiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434"); // Ollama
    client.Timeout = TimeSpan.FromMinutes(2);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowReactApp"); // 👈 ANTES de Authorization
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
