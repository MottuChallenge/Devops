using FluentValidation;
using MottuChallenge.Application.DTOs.Request;

namespace MottuChallenge.Application.DTOs.Validations
{
    public class SectorCreateDtoValidator : AbstractValidator<SectorCreateDto>
    {
        public SectorCreateDtoValidator()
        {
            RuleFor(x => x.YardId)
                .NotEmpty().WithMessage("O YardId é obrigatório.");

            RuleFor(x => x.SectorTypeId)
                .NotEmpty().WithMessage("O SectorTypeId é obrigatório.");

            RuleFor(x => x.Points)
                .NotEmpty().WithMessage("O setor deve ter pelo menos um ponto definido.")
                .Must(points => points.Count >= 3)
                .WithMessage("O setor deve ter no mínimo 3 pontos.");

            RuleForEach(x => x.Points).SetValidator(new CreatePolygonPointDtoValidator());
        }
    }
}
