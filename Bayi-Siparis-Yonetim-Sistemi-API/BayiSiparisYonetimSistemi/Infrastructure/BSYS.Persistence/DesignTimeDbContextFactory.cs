using BSYS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BSYS.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BSYSDbContext>
{
    public BSYSDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<BSYSDbContext> dbContextOptionsBuilder = new();
        dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
        return new(dbContextOptionsBuilder.Options);
    }
}
