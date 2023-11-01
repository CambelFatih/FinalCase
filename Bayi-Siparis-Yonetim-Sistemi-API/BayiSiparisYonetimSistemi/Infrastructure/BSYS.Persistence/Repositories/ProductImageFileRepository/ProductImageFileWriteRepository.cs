
using BSYS.Persistence.Contexts;
using BSYS.Domain.Entities;
using BSYS.Application.Repositories.ProductImageFileRepository;

namespace BSYS.Persistence.Repositories.ProductImageFileRepository;

public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
{
    public ProductImageFileWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
