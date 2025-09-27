using FluentValidation;
using MottuChallenge.Application.DTOs.Request;

namespace MottuChallenge.Application.DTOs.Validations
{
    public class SectorTypeDtoValidator : AbstractValidator<SectorTypeDto>
    {
        public SectorTypeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do tipo de setor é obrigatório.")
                .MaximumLength(50).WithMessage("O nome do tipo de setor deve ter no máximo 50 caracteres.");
        }
    }
}
