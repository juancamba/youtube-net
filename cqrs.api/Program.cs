using cqrs.api.Abtractions;
using cqrs.api.Features.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//builder.Services.AddScoped<ICommandBus, CommandBus>();
//builder.Services.AddScoped<IQueryBus, QueryBus>();

builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetUserByIdQuery, UserDto>, GetUserByIdQueryHandler>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();




//How to Implement the CQRS Pattern in Clean Architecture (from scratch)

app.MapPost("/user", async (CreateUserCommand user, IMediator mediator)=>{

    await mediator.SendAsync(user);
    return Results.Created();
});

app.MapGet("/user", async (int id, IMediator mediator)=>{
    var query = new GetUserByIdQuery(id);
    var result = await mediator.SendAsync<GetUserByIdQuery, UserDto>(query);
    return Results.Ok(result);
});

app.Run();

