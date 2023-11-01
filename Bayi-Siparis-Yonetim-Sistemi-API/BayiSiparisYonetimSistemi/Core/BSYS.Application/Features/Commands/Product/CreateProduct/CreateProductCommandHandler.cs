using BSYS.Application.Abstractions.Hubs;
using BSYS.Application.Abstractions.UoW;
using MediatR;


namespace BSYS.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductHubService _productHubService;
    private readonly IUnitofWork _uow;

    public CreateProductCommandHandler(IProductHubService productHubService, IUnitofWork uow)
    {
        _productHubService = productHubService;
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
        await _productHubService.ProductAddedMessageAsync($"{request.Name} isminde ürün eklenmiştir.");
        return new();
    }
}