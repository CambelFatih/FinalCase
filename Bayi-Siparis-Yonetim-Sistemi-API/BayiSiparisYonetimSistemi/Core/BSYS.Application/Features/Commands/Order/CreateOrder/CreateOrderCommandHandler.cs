using BSYS.Application.Abstractions.Hubs;
using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.UoW;
using MediatR;

namespace BSYS.Application.Features.Commands.Order.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;

    public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService)
    {
        _orderService = orderService;
        _basketService = basketService;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _orderService.CreateOrderAsync(new()
        {
            Address = request.Address,
            Description = request.Description,
            BasketId = _basketService.GetUserActiveBasket?.Id.ToString()
        });
        return new();
    }
}
