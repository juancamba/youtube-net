using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWithIA.Helpers;
using ApiWithIA.Models;
using ApiWithIA.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithIA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly AiService _ai;
        


        public QueryController(TicketService ticketService, AiService ai)
        {
            _ticketService = ticketService;
            _ai = ai;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserQuestion userQuestion)
        {
            // ia interpreta la pregunta
            var intent = await _ai.AnalyzeQuery(userQuestion.Question);

            if (intent == null)
                return BadRequest("No se pudo interpretar la pregunta.");

            
            DateTime? fechaInicio = intent.FechaInicio != null ? DateTime.Parse(intent.FechaInicio) : null;
            DateTime? fechaFin =  intent.FechaInicio != null ? DateTime.Parse(intent.FechaFin) : null;

            // Filtrar tickets usando TicketService
            var tickets = await _ticketService.GetTicketsAsync(intent.Categoria, fechaInicio, fechaFin);

            // Ejecutar operación
            decimal total = 0;
            if (intent.Operacion.ToLower() == "sum")
                total = tickets.Sum(t => t.Total);

            // Responder
            var respuesta = new
            {
                
                 mensaje = MensajeHelper.ConstruirMensaje(total, intent.Categoria, fechaInicio, fechaFin)
                //mensaje = $"Has gastado {total:C} en {intent.Categoria ?? "todas las categorías"} del {fechaInicio:MMMM yyyy} al {fechaFin:MMMM yyyy}"
            };

            return Ok(respuesta);
        }
    }
}