using BSYS.Application.Repositories.BasketRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.BasketRepository;

public class BasketReadRepository : ReadRepository<Basket>, IBasketReadRepository
{
    public BasketReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
