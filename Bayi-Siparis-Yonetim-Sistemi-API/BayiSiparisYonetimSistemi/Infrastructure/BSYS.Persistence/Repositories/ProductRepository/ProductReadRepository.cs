using BSYS.Application.Repositories.ProductRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.ProductRepository;

public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
{
    public ProductReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
