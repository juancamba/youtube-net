using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Domain.Abstractions;
public record Error(string Code, string Name)
{

    public static Error None = new(string.Empty, string.Empty);
    public static Error NullValue = new("Error.NullValue", "Un valor Null fue ingresado");

}
