using BSYS.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace BSYS.Application.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    DbSet<T> Table { get; }
}
