using FluentValidation;

namespace Tekus.Application.Features.Suppliers.GetAll
{
    public class GetSuppliersQueryValidator : AbstractValidator<GetSuppliersQuery>
    {
        public GetSuppliersQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
