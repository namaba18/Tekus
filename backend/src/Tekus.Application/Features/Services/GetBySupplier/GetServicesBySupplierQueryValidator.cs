using FluentValidation;

namespace Tekus.Application.Features.Services.GetBySupplier
{
    public class GetServicesBySupplierQueryValidator : AbstractValidator<GetServicesBySupplierQuery>
    {
        public GetServicesBySupplierQueryValidator()
        {
            RuleFor(x => x.SupplierId).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
