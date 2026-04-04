using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWithIA.Helpers
{
    public class MensajeHelper
    {
        public static string ConstruirMensaje(decimal total, string? categoria, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var cultura = new CultureInfo("es-ES");

            string FormatearMes(DateTime fecha) =>
                fecha.ToString("MMMM 'de' yyyy", cultura);

            string FormatearFechaCompleta(DateTime fecha) =>
                fecha.ToString("d 'de' MMMM 'de' yyyy", cultura);

            string rangoFechas = (fechaInicio, fechaFin) switch
            {
                // ✅ Mismo mes y año → formato corto
                (not null, not null) when fechaInicio.Value.Month == fechaFin.Value.Month
                                     && fechaInicio.Value.Year == fechaFin.Value.Year
                    => $"en {FormatearMes(fechaInicio.Value)}",

                // ✅ Rango completo con días
                (not null, not null)
                    => $"del {FormatearFechaCompleta(fechaInicio.Value)} al {FormatearFechaCompleta(fechaFin.Value)}",

                // ✅ Solo inicio
                (not null, null)
                    => $"desde {FormatearFechaCompleta(fechaInicio.Value)}",

                // ✅ Solo fin
                (null, not null)
                    => $"hasta {FormatearFechaCompleta(fechaFin.Value)}",

                _ => ""
            };

            var categoriaTexto = categoria ?? "todas las categorías";

            return $"Has gastado {total.ToString("C", cultura)} en {categoriaTexto}" +
                   (string.IsNullOrEmpty(rangoFechas) ? "" : $" {rangoFechas}");
        }
    }
}