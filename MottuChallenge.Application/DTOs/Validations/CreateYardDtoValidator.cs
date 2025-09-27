using FluentValidation;
using MottuChallenge.Application.DTOs.Request;

namespace MottuChallenge.Application.DTOs.Validations
{
    public class CreateYardDtoValidator : AbstractValidator<CreateYardDto>
    {
        public CreateYardDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do pátio é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do pátio deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{8}$").WithMessage("O CEP deve estar no formato válido (ex: 12345-678).");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("O número é obrigatório.");

            RuleFor(x => x.Points)
                 .NotEmpty().WithMessage("O pátio deve ter pelo menos um ponto definido.")
                 .Must(points => points.Count >= 3)
                 .WithMessage("O pátio deve ter no mínimo 3 pontos.");

            RuleForEach(x => x.Points).SetValidator(new CreatePolygonPointDtoValidator());
        }
    }
}
