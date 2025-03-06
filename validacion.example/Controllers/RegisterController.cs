using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace validacion.example.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest registerRequest, IValidator<RegisterRequest> validator)
        {
            var validacionResult = validator.Validate(registerRequest);
            
            if(!validacionResult.IsValid)
            {
                //mostrar los errores
                // Mostrar los errores de validación
                var errores = validacionResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errores });
            }
            
            
            // Aquí podrías agregar lógica para almacenar el usuario en la base de datos

            return Ok(new { Message = "User registered successfully" });


        }
    }
}