using BSYS.Application.Abstractions.UoW;
using MediatR;


namespace BSYS.Application.Features.Commands.Product.RemoveProduct;

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
{
    readonly IUnitofWork _uow;

    public RemoveProductCommandHandler(IUnitofWork uow)
    {
        _uow = uow;
    }

    public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _uow.ProductWriteRepository.RemoveAsync(request.Id);
        await _uow.ProductWriteRepository.SaveAsync();
        return new();
    }
}
