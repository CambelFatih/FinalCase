using BSYS.Domain.Entities;

namespace BSYS.Application.Repositories.BasketItemRepository;

public interface IBasketItemReadRepository : IReadRepository<BasketItem>
{
    public Task<List<BasketItem>> GetBasketItemsByBasketId(string basketId);
}
