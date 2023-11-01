
using BSYS.Application.Abstractions.UoW;
using BSYS.Application.Repositories.ProductRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BSYS.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
{

    private readonly IUnitofWork _uow;

    public RemoveProductImageCommandHandler(IUnitofWork uow)
    {
        _uow = uow;
    }

    public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product = await _uow.ProductReadRepository.Table.Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

        Domain.Entities.ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));

        if (productImageFile != null)
            product?.ProductImageFiles.Remove(productImageFile);

        await _uow.ProductWriteRepository.SaveAsync();
        return new();
    }
}
