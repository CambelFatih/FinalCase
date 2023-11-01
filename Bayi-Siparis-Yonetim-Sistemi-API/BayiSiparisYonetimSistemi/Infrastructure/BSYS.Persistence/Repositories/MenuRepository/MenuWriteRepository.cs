
using BSYS.Application.Repositories.MenuRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.MenuRepository;

public class MenuWriteRepository : WriteRepository<Menu>, IMenuWriteRepository
{
    public MenuWriteRepository(BSYSDbContext context) : base(context)
    {
    }
}
