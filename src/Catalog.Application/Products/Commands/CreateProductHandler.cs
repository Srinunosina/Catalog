using Catalog.Application.interfaces;
using Catalog.Application.Shared;
using Catalog.Application.Shared.Results;
using Catalog.Domain.Products;
using MediatR;

namespace Catalog.Application.Products.Commands;
public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _repository;

    public CreateProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var product = Product.Create("1.0", command.Name, command.Price);

        await _repository.AddAsync(product);

        return Result<Guid>.Success(product.Id);
    }
}
