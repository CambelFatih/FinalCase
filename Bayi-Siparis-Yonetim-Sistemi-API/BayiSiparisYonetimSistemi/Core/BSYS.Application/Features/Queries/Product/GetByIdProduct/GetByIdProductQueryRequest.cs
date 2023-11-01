using MediatR;

namespace BSYS.Application.Features.Queries.Product.GetByIdProduct;

public class GetByIdProductQueryRequest : IRequest<GetByIdProductQueryResponse>
{
    public string Id { get; set; }
}
