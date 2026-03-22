using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiWithIA.Data;
using ApiWithIA.Models;
using ApiWithIA.Services;
using Microsoft.AspNetCore.Mvc;
using Tesseract;

namespace ApiWithIA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly AiService _ai;
        private readonly AppDbContext _db;

        public TicketsController(AiService ai, AppDbContext db)
        {
            _ai = ai;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] string rawText)
        {
            var aiResult = await _ai.AnalyzeTicket(rawText);

            var ticketData = JsonSerializer.Deserialize<Ticket>(aiResult);

            _db.Tickets.Add(ticketData);
            await _db.SaveChangesAsync();

            return Ok(ticketData);
        }


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se subió ningún archivo.");


            var tempPath = Path.GetTempFileName();
            await using (var stream = System.IO.File.Create(tempPath))
            {
                await file.CopyToAsync(stream);
            }


            string extractedText;
            var tessPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");

            using (var engine = new Tesseract.TesseractEngine(tessPath, "spa", EngineMode.Default))
            {
                using (var img = Tesseract.Pix.LoadFromFile(tempPath))
                using (var page = engine.Process(img))
                {
                    extractedText = page.GetText();
                }
            }

            // Analizar el texto con IA
            var aiResult = await _ai.AnalyzeTicket(extractedText);

            // aiResult = raw string de la respuesta de Ollama
            using var doc = JsonDocument.Parse(aiResult);

            var responseText = doc.RootElement.GetProperty("response").GetString();

            var ticketDto = JsonSerializer.Deserialize<TicketDto>(responseText);


            Ticket ticketData = Ticket.FromDto(ticketDto!, responseText);

            _db.Tickets.Add(ticketData);
            await _db.SaveChangesAsync();


            System.IO.File.Delete(tempPath);

            return Ok(ticketData);
        }
    }
}