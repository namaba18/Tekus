namespace Tekus.Application.Features.Suppliers.GetAll
{
    public record SupplierResponse(Guid Id, string NIT, string Name, string WebPage, string Email);
}
