
using BSYS.Application.Repositories.ProductRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.ProductRepository;

public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
{
    public ProductWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
