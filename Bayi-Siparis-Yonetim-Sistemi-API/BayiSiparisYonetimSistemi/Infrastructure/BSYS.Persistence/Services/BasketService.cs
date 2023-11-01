using BSYS.Application.ViewModels.Baskets;
using BSYS.Domain.Entities;
using BSYS.Domain.Entities.Identity;
using BSYS.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BSYS.Application.Abstractions.UoW;

namespace BSYS.Persistence.Services;

public class BasketService : IBasketService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitofWork _uow;
    public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IUnitofWork uow)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _uow = uow;
    }

    private async Task<Basket?> ContextUser()
    {
        var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(username))
        {
            AppUser? user = await _userManager.Users
                     .Include(u => u.Baskets)
                     .FirstOrDefaultAsync(u => u.UserName == username);

            var _basket = from basket in user.Baskets
                          join order in _uow.OrderReadRepository.Table
                          on basket.Id equals order.Id into BasketOrders
                          from order in BasketOrders.DefaultIfEmpty()
                          select new
                          {
                              Basket = basket,
                              Order = order
                          };

            Basket? targetBasket = null;
            if (_basket.Any(b => b.Order is null))
                targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
            else
            {
                targetBasket = new();
                user.Baskets.Add(targetBasket);
            }

            await _uow.BasketWriteRepository.SaveAsync();
            return targetBasket;
        }
        throw new Exception("Beklenmeyen bir hatayla karşılaşıldı...");
    }

    public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
    {
        Basket? basket = await ContextUser();
        if (basket != null)
        {
            BasketItem _basketItem = await _uow.BasketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
            if (_basketItem != null)
                _basketItem.Quantity++;
            else
                await _uow.BasketItemWriteRepository.AddAsync(new()
                {
                    BasketId = basket.Id,
                    ProductId = Guid.Parse(basketItem.ProductId),
                    Quantity = basketItem.Quantity
                });

            await _uow.BasketItemWriteRepository.SaveAsync();
        }
    }

    public async Task<List<BasketItem>> GetBasketItemsAsync()
    {
        Basket? basket = await ContextUser();
        Basket? result = await _uow.BasketReadRepository.Table
             .Include(b => b.BasketItems)
             .ThenInclude(bi => bi.Product)
             .FirstOrDefaultAsync(b => b.Id == basket.Id);

        return result.BasketItems
            .ToList();
    }

    public async Task RemoveBasketItemAsync(string basketItemId)
    {
        BasketItem? basketItem = await _uow.BasketItemReadRepository.GetByIdAsync(basketItemId);
        if (basketItem != null)
        {
            _uow.BasketItemWriteRepository.Remove(basketItem);
            await _uow.BasketItemWriteRepository.SaveAsync();
        }
    }

    public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
    {
        BasketItem? _basketItem = await _uow.BasketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
        if (_basketItem != null)
        {
            _basketItem.Quantity = basketItem.Quantity;
            await _uow.BasketItemWriteRepository.SaveAsync();
        }
    }

    public Basket? GetUserActiveBasket
    {
        get
        {
            Basket? basket = ContextUser().Result;
            return basket;
        }
    }
}
