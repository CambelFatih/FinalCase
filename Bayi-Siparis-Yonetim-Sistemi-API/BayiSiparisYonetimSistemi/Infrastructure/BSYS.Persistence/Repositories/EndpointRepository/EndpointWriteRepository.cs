
using BSYS.Persistence.Contexts;
using BSYS.Domain.Entities;
using BSYS.Application.Repositories.EndpointRepository;

namespace BSYS.Persistence.Repositories.EndpointRepository;

public class EndpointWriteRepository : WriteRepository<Endpoint>, IEndpointWriteRepository
{
    public EndpointWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
