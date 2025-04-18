using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cqrs.api.Abtractions;

namespace cqrs.api.Features.Users;

public record CreateUserCommand(string Email , string Username) : ICommand;

