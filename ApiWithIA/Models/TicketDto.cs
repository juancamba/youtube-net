using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiWithIA.Models;
public record TicketDto(
    [property: JsonPropertyName("fecha")] DateTime Fecha,
    [property: JsonPropertyName("empresa")] string Empresa,
    [property: JsonPropertyName("total")] decimal Total,
    [property: JsonPropertyName("categoria")] string Categoria
);
