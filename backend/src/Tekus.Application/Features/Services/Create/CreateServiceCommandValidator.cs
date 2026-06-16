using FluentValidation;

namespace Tekus.Application.Features.Services.Create
{
    public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.HourlyRate).GreaterThan(0);
            RuleFor(x => x.SupplierId).NotEmpty();
        }
    }
}
