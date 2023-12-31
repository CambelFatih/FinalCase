﻿using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.UoW;
using MediatR;

namespace BSYS.Application.Features.Queries.Order.GetAllOrders;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
{
    private readonly IOrderService _orderService;

    public GetAllOrdersQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
    {
        var data = await _orderService.GetAllOrdersAsync(request.Page, request.Size);

        return new()
        {
            TotalOrderCount = data.TotalOrderCount,
            Orders = data.Orders
        };
    }
}
