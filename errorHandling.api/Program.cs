using Microsoft.AspNetCore.Diagnostics;

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

app.MapGet("/product", ()=>{

    throw new Exception("Some kind of error");    
    

});

app.MapPost("/product", (ProductRequest request)=>{
    var product = new ProductRequest(1, "Teclado logitech");
     
     return Results.Created($"/product/{product.Id}", product);
});


app.UseExceptionHandler(appError=>{
    appError.Run(async context=>{
        context.Response.StatusCode = 500;
        context.Response.ContentType ="application/json";
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

        if(contextFeature is not null)
        {
            Console.WriteLine($"Error : {contextFeature.Error}");
            await context.Response.WriteAsJsonAsync(new {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error",
                MoreInfo = contextFeature.Error.Message
            });
        }
    });

});



app.Run();

public record ProductRequest (int Id, string Name);
