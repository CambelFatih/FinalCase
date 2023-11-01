
using BSYS.Persistence.Contexts;
using BSYS.Domain.Entities;
using BSYS.Application.Repositories.OrderRepository;

namespace BSYS.Persistence.Repositories.OrderRepository;

public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository
{
    public OrderReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
