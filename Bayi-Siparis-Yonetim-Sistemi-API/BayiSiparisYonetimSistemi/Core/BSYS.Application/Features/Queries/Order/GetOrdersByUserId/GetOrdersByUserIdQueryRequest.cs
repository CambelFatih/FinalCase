
using MediatR;

namespace BSYS.Application.Features.Queries.Order.GetOrdersByUserId;

public class GetOrdersByUserIdQueryRequest : IRequest<GetOrdersByUserIdQueryResponse>
{
    public string UserId { get; set; }
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
