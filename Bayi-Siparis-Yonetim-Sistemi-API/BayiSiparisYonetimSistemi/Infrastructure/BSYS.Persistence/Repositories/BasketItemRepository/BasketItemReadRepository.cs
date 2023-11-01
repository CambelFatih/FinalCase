using BSYS.Application.Repositories.BasketItemRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BSYS.Persistence.Repositories.BasketItemRepository;

public class BasketItemReadRepository : ReadRepository<BasketItem>, IBasketItemReadRepository
{
    private readonly BSYSDbContext _context;
    public BasketItemReadRepository(BSYSDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<BasketItem>> GetBasketItemsByBasketId(string basketId)
    {
        // Veritabanından basketId ile ilişkilendirilen tüm BasketItem nesnelerini alın
        return await _context.BasketItems.Where(item => item.BasketId == Guid.Parse(basketId)).ToListAsync();
    }

}
