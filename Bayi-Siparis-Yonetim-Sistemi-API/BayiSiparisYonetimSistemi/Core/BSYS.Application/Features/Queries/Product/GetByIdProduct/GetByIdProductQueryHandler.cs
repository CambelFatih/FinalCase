
using BSYS.Application.Abstractions.UoW;
using BSYS.Application.Repositories.ProductRepository;
using MediatR;

namespace BSYS.Application.Features.Queries.Product.GetByIdProduct;

internal class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
{

    private readonly IUnitofWork _uow;

    public GetByIdProductQueryHandler(IUnitofWork uow)
    {
        _uow = uow;
    }

    public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _uow.ProductReadRepository.GetByIdAsync(request.Id, false);
        return new()
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}
