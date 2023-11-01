
using BSYS.Application.Repositories.EndpointRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.EndpointRepository;

public class EndpointReadRepository : ReadRepository<Endpoint>, IEndpointReadRepository
{
    public EndpointReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
