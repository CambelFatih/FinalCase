using BSYS.Application.Abstractions.Hubs;
using BSYS.Application.Abstractions.UoW;
using MediatR;


namespace BSYS.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IUnitofWork _uow;

    public CreateProductCommandHandler( IUnitofWork uow)
    {
        _uow = uow;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _uow.ProductWriteRepository.AddAsync(new()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        });
        await _uow.ProductWriteRepository.SaveAsync();
        return new();
    }
}