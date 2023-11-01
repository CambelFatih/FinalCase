
using BSYS.Application.Repositories.ProductImageFileRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.ProductImageFileRepository;

public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
{
    public ProductImageFileReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
