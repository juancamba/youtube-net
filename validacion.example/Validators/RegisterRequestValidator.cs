using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;
using validacion.example.Controllers;


namespace validacion.example.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("Nombre is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.FechaNacimiento)
                .NotEmpty().WithMessage("FechaNacimiento is required")
                .Must(fecha => fecha.AddYears(18) <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("User must be at least 18 years old");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(10).WithMessage("Password must be Gt 10")
                ;

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}