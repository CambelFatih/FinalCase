using BSYS.Domain.Entities.Common;

namespace BSYS.Domain.Entities;

public class Menu : BaseEntity
{
    public string Name { get; set; }

    public ICollection<Endpoint> Endpoints { get; set; }
}
