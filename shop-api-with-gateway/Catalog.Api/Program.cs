var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/catalog", () =>
{
    return new[]
    {
        new { Id = 1, Product = "Laptop" },
        new { Id = 2, Product = "Mouse" }
    };
});



app.Run();

