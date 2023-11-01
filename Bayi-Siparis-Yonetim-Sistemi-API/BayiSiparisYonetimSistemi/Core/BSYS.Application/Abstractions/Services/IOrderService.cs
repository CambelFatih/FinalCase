using BSYS.Application.DTOs.Order;

namespace BSYS.Application.Abstractions.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
    Task<ListOrder> GetAllOrdersAsync(int page, int size);
    Task<ListOrder> GetOrdersByUserIdAsync(string userId, int page, int size);
    Task<SingleOrder> GetOrderByIdAsync(string id);
    Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
}
