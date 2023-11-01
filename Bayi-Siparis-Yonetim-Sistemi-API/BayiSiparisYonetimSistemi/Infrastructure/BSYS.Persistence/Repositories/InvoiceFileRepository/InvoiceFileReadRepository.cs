
using BSYS.Persistence.Contexts;
using BSYS.Domain.Entities;
using BSYS.Application.Repositories.InvoiceFileRepository;

namespace BSYS.Persistence.Repositories.InvoiceFileRepository;

public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
{
    public InvoiceFileReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
