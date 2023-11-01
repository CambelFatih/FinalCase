using BSYS.Application.Repositories.BasketRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.BasketRepository;

public class BasketWriteRepository : WriteRepository<Basket>, IBasketWriteRepository
{
    public BasketWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
