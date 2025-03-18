using FluentValidation;
using products.api.Models.Dto;

namespace products.api.Validators
{
    public class ProductRequestDtoValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("Nombre is required");
            RuleFor(x => x.Description)
                  .NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
}
