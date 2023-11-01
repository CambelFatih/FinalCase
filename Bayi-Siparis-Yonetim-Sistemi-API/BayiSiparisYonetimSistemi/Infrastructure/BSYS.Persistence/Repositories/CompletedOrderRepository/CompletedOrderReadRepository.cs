using BSYS.Persistence.Contexts;
using BSYS.Domain.Entities;
using BSYS.Application.Repositories.CompletedOrderRepository;

namespace BSYS.Persistence.Repositories.CompletedOrderRepository;

public class CompletedOrderReadRepository : ReadRepository<CompletedOrder>, ICompletedOrderReadRepository
{
    public CompletedOrderReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
