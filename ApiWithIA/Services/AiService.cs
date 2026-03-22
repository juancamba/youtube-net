using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiWithIA.Models;

namespace ApiWithIA.Services
{
    public class AiService
    {
        private readonly HttpClient _http;
        private const string MODEL = "llama3";
        const string PROMPT_ANALYZE_TICKET = @"
                Devuelve SOLO un JSON válido. Sin explicaciones. Sin texto adicional.


                Formato exacto:
                {{
                ""fecha"": ""YYYY-MM-DD"",
                ""empresa"": ""string"",
                ""total"": number,
                ""categoria"": ""comida|supermercado|restaurante|gasolina|otros""
                }}

                Si no encuentras un dato, usa null.

                Ticket: 
                {query_ticket}
                
                ";
        const string PROMPT_QUERY = @"
            Convierte la siguiente pregunta en un JSON EXACTO:

            {
            ""categoria"": string | null,
            ""fecha_inicio"": string (YYYY-MM-DD) | null,
            ""fecha_fin"": string (YYYY-MM-DD) | null,
            ""operacion"": ""sum""
            }

            Categorías posibles: supermercado, comida, gasolina, restaurante, otros.

            Sinónimos:
            - ""super"" → ""supermercado""
            - ""gas"" → ""gasolina""
            - ""restaurante"" → ""restaurante""
            - ""comida"" → ""comida""

            Reglas de interpretación de fechas:
            - ""este mes"" → primer y último día del mes actual
            - ""mes pasado"" → primer y último día del mes anterior
            - Meses mencionados explícitamente (""enero"", ""abril"", etc.) → primer y último día de ese mes del año actual
            - Expresiones como ""última semana"", ""los últimos 7 días"" → convertir automáticamente a fechas
            - Si el usuario menciona año explícito, úsalo; si no, usa el año actual

            Responde SOLO en JSON válido, sin explicaciones.

            Ejemplos:

            Pregunta: ""¿Cuánto gasté en gasolina en marzo?""
            Respuesta: {""categoria"":""gasolina"",""fecha_inicio"":""2026-03-01"",""fecha_fin"":""2026-03-31"",""operacion"":""sum""}

            Pregunta: ""Gasto en supermercado el mes de abril""
            Respuesta: {""categoria"":""supermercado"",""fecha_inicio"":""2026-04-01"",""fecha_fin"":""2026-04-30"",""operacion"":""sum""}

            Pregunta: ""Gasto en comida el mes de abril""
            Respuesta: {""categoria"":""comida"",""fecha_inicio"":""2026-04-01"",""fecha_fin"":""2026-04-30"",""operacion"":""sum""}

            Pregunta: ""¿Cuánto gasté en restaurantes la última semana?""
            Respuesta: {""categoria"":""restaurante"",""fecha_inicio"":""2026-03-15"",""fecha_fin"":""2026-03-21"",""operacion"":""sum""}

            Pregunta: ""{user_question}""
            ";
        public AiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> AnalyzeTicket(string text)
        {

            string prompt = PROMPT_ANALYZE_TICKET.Replace("{query_ticket}", text);

            var body = new
            {
                model = MODEL,
                prompt = prompt,
                stream = false,
                options = new
                {
                    temperature = 0,
                    num_predict = 150
                }
            };

            var response = await _http.PostAsJsonAsync("/api/generate", body);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<QueryIntent> AnalyzeQuery(string text)
        {
            string prompt = PROMPT_QUERY.Replace("{user_question}", text);

            var body = new
            {
                model = MODEL,
                prompt = prompt,
                temperature = 0.0
            };

            var response = await _http.PostAsJsonAsync("/api/generate", body);

            var content = await response.Content.ReadAsStringAsync();


            // Dividir por líneas (cada línea es un fragmento JSON de Llama)
            var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var jsonBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                try
                {
                    using var doc = JsonDocument.Parse(line);
                    if (doc.RootElement.TryGetProperty("response", out var resp))
                    {
                        jsonBuilder.Append(resp.GetString());
                    }
                }
                catch
                {
                    // Ignorar líneas que no sean JSON válidas
                }
            }

            var finalJson = jsonBuilder.ToString();

            // Limpiar posibles caracteres extra al inicio o final
            finalJson = finalJson.Trim();

            try
            {
                // Llama 3 devuelve un string con JSON
                return JsonSerializer.Deserialize<QueryIntent>(finalJson);
            }
            catch
            {
                return null;
            }
        }
    }
}