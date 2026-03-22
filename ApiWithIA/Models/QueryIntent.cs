using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiWithIA.Models;
public class QueryIntent
{
    [JsonPropertyName("categoria")]
    public string Categoria { get; set; } // supermercado, gasolina, restaurante, otros

    [JsonPropertyName("fecha_inicio")]
    public string FechaInicio { get; set; } // YYYY-MM-DD

    [JsonPropertyName("fecha_fin")]
    public string FechaFin { get; set; } // YYYY-MM-DD

    [JsonPropertyName("operacion")]
    public string Operacion { get; set; } // sum
}
