using BSYS.Application.Repositories.BasketItemRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.BasketItemRepository;

public class BasketItemWriteRepository : WriteRepository<BasketItem>, IBasketItemWriteRepository
{
    public BasketItemWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
