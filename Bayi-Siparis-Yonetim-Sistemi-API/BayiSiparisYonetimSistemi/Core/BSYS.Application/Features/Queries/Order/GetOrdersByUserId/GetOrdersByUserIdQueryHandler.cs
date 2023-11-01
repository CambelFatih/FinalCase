using BSYS.Application.Abstractions.Services;
using BSYS.Application.Features.CQRS;
using MediatR;

namespace BSYS.Application.Features.Queries.Order.GetOrdersByUserId;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQueryRequest, GetOrdersByUserIdQueryResponse>
{
    private readonly IOrderService _orderService;

    public GetOrdersByUserIdQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<GetOrdersByUserIdQueryResponse> Handle(GetOrdersByUserIdQueryRequest request, CancellationToken cancellationToken)
    {
        var data = await _orderService.GetOrdersByUserIdAsync(request.UserId,request.Page, request.Size);

        return new()
        {
            TotalOrderCount = data.TotalOrderCount,
            Orders = data.Orders
        };
    }
}
