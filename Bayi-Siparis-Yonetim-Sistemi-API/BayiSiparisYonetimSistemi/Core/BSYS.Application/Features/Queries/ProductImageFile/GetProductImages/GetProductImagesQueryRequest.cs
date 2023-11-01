using MediatR;

namespace BSYS.Application.Features.Queries.ProductImageFile.GetProductImages;

public class GetProductImagesQueryRequest : IRequest<List<GetProductImagesQueryResponse>>
{
    public string Id { get; set; }
}
