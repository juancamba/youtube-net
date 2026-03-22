using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiWithIA.Models;

public class Ticket
{
    public int Id { get; set; }
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }
    [JsonPropertyName("empresa")]
    public string Empresa { get; set; }
    [JsonPropertyName("total")]
    public decimal Total { get; set; }

    [JsonPropertyName("categoria")]
    public string Categoria { get; set; }
    public string RawText { get; set; }

    public static Ticket FromDto(TicketDto dto, string rawText = null)
    {
        return new Ticket
        {
            Fecha = dto.Fecha,
            Empresa = dto.Empresa,
            Total = dto.Total,
            Categoria = dto.Categoria,
            RawText = rawText
        };
    }


}

