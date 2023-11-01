
using BSYS.Persistence.Contexts;
using BSYS.Domain.Entities;
using BSYS.Application.Repositories.InvoiceFileRepository;

namespace BSYS.Persistence.Repositories.InvoiceFileRepository;

public class InvoiceFileWriteRepository : WriteRepository<InvoiceFile>, IInvoiceFileWriteRepository
{
    public InvoiceFileWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
