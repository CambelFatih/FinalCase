using BSYS.Application.Repositories.CustomerRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.CustomerRepository;

public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
{
    public CustomerWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
