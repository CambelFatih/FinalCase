using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.UoW;
using BSYS.Application.DTOs.Order;
using BSYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSYS.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IUnitofWork _uow;

    public OrderService(IUnitofWork uow)
    {
        _uow = uow;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        var orderCode = GenerateOrderCode();

        // Sipariş oluştur
        var order = new Order
        {
            Address = createOrder.Address,
            Id = Guid.Parse(createOrder.BasketId),
            Description = createOrder.Description,
            OrderCode = orderCode
        };
        await _uow.OrderWriteRepository.AddAsync(order);

        // Siparişe ait ürünleri al
        var basketItems = await _uow.BasketItemReadRepository.GetBasketItemsByBasketId(createOrder.BasketId);

        // Her bir ürünün stock bilgisini güncelle
        foreach (var basketItem in basketItems)
        {
            var product = await _uow.ProductReadRepository.GetByIdAsync(basketItem.ProductId.ToString());

            if (product != null)
            {
                // Ürünün stokunu güncelle
                product.Stock -= basketItem.Quantity;
                _uow.ProductWriteRepository.Update(product);
            }
        }
        await _uow.OrderWriteRepository.SaveAsync();
        await _uow.ProductWriteRepository.SaveAsync();
    }

    private string GenerateOrderCode()
    {
        var orderCode = (new Random().NextDouble() * 10000).ToString();
        return orderCode.Substring(orderCode.IndexOf(".") + 1, orderCode.Length - orderCode.IndexOf(".") - 1);
    }
    public async Task<ListOrder> GetOrdersByUserIdAsync(string userId, int page, int size)
    {
        var query = _uow.OrderReadRepository.Table
            .Include(o => o.Basket)
            .ThenInclude(b => b.User)
            .Include(o => o.Basket)
            .ThenInclude(b => b.BasketItems)
            .ThenInclude(bi => bi.Product)
            .Where(o => o.Basket.UserId == userId);

        var data = query.Skip(page * size).Take(size);

        var data2 = from order in data
                    join completedOrder in _uow.CompletedOrderReadRepository.Table
                    on order.Id equals completedOrder.OrderId into co
                    from _co in co.DefaultIfEmpty()
                    select new
                    {
                        order.Id,
                        order.CreatedDate,
                        order.OrderCode,
                        order.Basket,
                        Completed = _co != null ? true : false
                    };

        return new ListOrder
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data2.Select(o => new
            {
                o.Id,
                o.CreatedDate,
                o.OrderCode,
                TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                o.Basket.User.UserName,
                o.Completed
            }).ToListAsync()
        };
    }


    public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
    {
        var query = _uow.OrderReadRepository.Table.Include(o => o.Basket)
                  .ThenInclude(b => b.User)
                  .Include(o => o.Basket)
                     .ThenInclude(b => b.BasketItems)
                     .ThenInclude(bi => bi.Product);



        var data = query.Skip(page * size).Take(size);
        /*.Take((page * size)..size);*/


        var data2 = from order in data
                    join completedOrder in _uow.CompletedOrderReadRepository.Table
                       on order.Id equals completedOrder.OrderId into co
                    from _co in co.DefaultIfEmpty()
                    select new
                    {
                        order.Id,
                        order.CreatedDate,
                        order.OrderCode,
                        order.Basket,
                        Completed = _co != null ? true : false
                    };

        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data2.Select(o => new
            {
                o.Id,
                o.CreatedDate,
                o.OrderCode,
                TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                o.Basket.User.UserName,
                o.Completed
            }).ToListAsync()
        };
    }

    public async Task<SingleOrder> GetOrderByIdAsync(string id)
    {
        var data = _uow.OrderReadRepository.Table
                             .Include(o => o.Basket)
                                 .ThenInclude(b => b.BasketItems)
                                     .ThenInclude(bi => bi.Product);

        var data2 = await (from order in data
                           join completedOrder in _uow.CompletedOrderReadRepository.Table
                                on order.Id equals completedOrder.OrderId into co
                           from _co in co.DefaultIfEmpty()
                           select new
                           {
                               order.Id,
                               order.CreatedDate,
                               order.OrderCode,
                               order.Basket,
                               Completed = _co != null ? true : false,
                               order.Address,
                               order.Description
                           }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        return new()
        {
            Id = data2.Id.ToString(),
            BasketItems = data2.Basket.BasketItems.Select(bi => new
            {
                bi.Product.Name,
                bi.Product.Price,
                bi.Quantity
            }),
            Address = data2.Address,
            CreatedDate = data2.CreatedDate,
            Description = data2.Description,
            OrderCode = data2.OrderCode,
            Completed = data2.Completed
        };
    }

    public async Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id)
    {
        Order? order = await _uow.OrderReadRepository.Table
            .Include(o => o.Basket)
            .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        if (order != null)
        {
            await _uow.CompletedOrderWriteRepository.AddAsync(new() { OrderId = Guid.Parse(id) });
            return (await _uow.CompletedOrderWriteRepository.SaveAsync() > 0, new()
            {
                OrderCode = order.OrderCode,
                OrderDate = order.CreatedDate,
                Username = order.Basket.User.UserName,
                EMail = order.Basket.User.Email
            });
        }
        return (false, null);
    }
}
