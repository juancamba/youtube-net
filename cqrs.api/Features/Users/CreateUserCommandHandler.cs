using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cqrs.api.Abtractions;

namespace cqrs.api.Features.Users;
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public async Task HandleAsync(CreateUserCommand command)
    {
        // call repository

        //send some evento to kafka 

        // send email


        Console.WriteLine($"Creando usuario: {command.Username}");
        await Task.CompletedTask;
    }
}
