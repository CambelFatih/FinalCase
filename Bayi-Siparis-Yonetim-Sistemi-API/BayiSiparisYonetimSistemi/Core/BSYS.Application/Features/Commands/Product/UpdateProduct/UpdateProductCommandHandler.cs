
using BSYS.Application.Abstractions.UoW;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSYS.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IUnitofWork _uow;
    private readonly ILogger<UpdateProductCommandHandler> logger;

    public UpdateProductCommandHandler(IUnitofWork uow, ILogger<UpdateProductCommandHandler> logger)
    {
        _uow = uow;
        this.logger = logger;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _uow.ProductReadRepository.GetByIdAsync(request.Id);
        product.Stock = request.Stock;
        product.Name = request.Name;
        product.Price = request.Price;
        await _uow.ProductWriteRepository.SaveAsync();
        logger.LogInformation("Product güncellendi...");
        return new();
    }
}
