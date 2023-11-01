
using BSYS.Application.Repositories.FileRepository;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.FileRepository;

public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
{
    public FileWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
