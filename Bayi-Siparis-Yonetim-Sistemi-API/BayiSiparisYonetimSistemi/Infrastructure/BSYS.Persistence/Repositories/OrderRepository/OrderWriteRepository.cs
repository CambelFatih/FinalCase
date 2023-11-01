
using BSYS.Application.Repositories.OrderRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.OrderRepository;

public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
{
    public OrderWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
