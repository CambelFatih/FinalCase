
using BSYS.Application.Repositories.MenuRepository;
using BSYS.Domain.Entities;
using BSYS.Persistence.Contexts;

namespace BSYS.Persistence.Repositories.MenuRepository;

public class MenuReadRepository : ReadRepository<Menu>, IMenuReadRepository
{
    public MenuReadRepository(BSYSDbContext context) : base(context)
    {
    }
}
