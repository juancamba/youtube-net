using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace products.api.Services
{
    public class ApiDummyHealth : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        public ApiDummyHealth(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var url = "https://jsonplaceholder1.typicode.com/posts/1";

            var respuesta = await _httpClient.GetAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();

        }
    }
}