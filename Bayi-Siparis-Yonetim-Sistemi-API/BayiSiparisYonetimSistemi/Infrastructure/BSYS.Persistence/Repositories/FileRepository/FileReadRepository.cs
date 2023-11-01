
using BSYS.Application.Repositories.FileRepository;
using BSYS.Persistence.Contexts;


namespace BSYS.Persistence.Repositories.FileRepository;

public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
{
    public FileReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
