using FluentValidation;
using MottuChallenge.Application.DTOs.Request;

namespace MottuChallenge.Application.DTOs.Validations
{
    public class CreatePolygonPointDtoValidator : AbstractValidator<CreatePolygonPointDto>
    {
        public CreatePolygonPointDtoValidator()
        {
            RuleFor(x => x.PointOrder)
                .GreaterThan(0).WithMessage("A ordem do ponto deve ser maior que 0.");

            RuleFor(x => x.X)
                .NotNull().WithMessage("O valor X do ponto é obrigatório.");

            RuleFor(x => x.Y)
                .NotNull().WithMessage("O valor Y do ponto é obrigatório.");
        }
    }
}
