using BSYS.Application.Abstractions.Storage;
using BSYS.Application.Abstractions.UoW;
using MediatR;

namespace BSYS.Application.Features.Commands.ProductImageFile.UploadProductImage;

public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUnitofWork _uow;

    public UploadProductImageCommandHandler(IStorageService storageService, IUnitofWork uow)
    {
        _storageService = storageService;
        _uow = uow;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.Files);


        Domain.Entities.Product product = await _uow.ProductReadRepository.GetByIdAsync(request.Id);


        await _uow.ProductImageFileWriteRepository.AddRangeAsync(result.Select(r => new Domain.Entities.ProductImageFile
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() { product }
        }).ToList());

        await _uow.ProductImageFileWriteRepository.SaveAsync();

        return new();
    }
}
