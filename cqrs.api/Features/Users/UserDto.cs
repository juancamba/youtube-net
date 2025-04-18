using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cqrs.api.Features.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
    }
}