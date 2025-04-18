using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cqrs.api.Abtractions;

namespace cqrs.api.Features.Users
{


    public record GetUserByIdQuery(int UserId) : IQuery<UserDto>;
    

    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {

        public async Task<UserDto> HandleAsync(
            GetUserByIdQuery query)
        {
            // lógica para obtener usuario
            return await Task.FromResult(new 
            UserDto { Id = query.UserId, 
            Username = "Juanito" });
        }
    }
}