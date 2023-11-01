using BSYS.Application.Repositories.CompletedOrderRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.CompletedOrderRepository;

public class CompletedOrderWriteRepository : WriteRepository<CompletedOrder>, ICompletedOrderWriteRepository
{
    public CompletedOrderWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
