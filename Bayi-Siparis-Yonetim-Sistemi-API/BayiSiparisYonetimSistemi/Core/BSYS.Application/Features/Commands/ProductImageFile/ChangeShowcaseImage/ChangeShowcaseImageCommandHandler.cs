
using BSYS.Application.Abstractions.UoW;
using Microsoft.EntityFrameworkCore;

namespace BSYS.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;

public class ChangeShowcaseImageCommandHandler : MediatR.IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
{
    private readonly IUnitofWork _uow;

    public ChangeShowcaseImageCommandHandler(IUnitofWork uow)
    {
        _uow = uow;
    }

    public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
    {
        var query = _uow.ProductImageFileWriteRepository.Table
                  .Include(p => p.Products)
                  .SelectMany(p => p.Products, (pif, p) => new
                  {
                      pif,
                      p
                  });

        var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase);

        if (data != null)
            data.pif.Showcase = false;

        var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(request.ImageId));
        if (image != null)
            image.pif.Showcase = true;

        await _uow.ProductImageFileWriteRepository.SaveAsync();

        return new();
    }
}
