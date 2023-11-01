using BSYS.Domain.Entities.Common;
using BSYS.Domain.Entities.Identity;

namespace BSYS.Domain.Entities;

public class Basket : BaseEntity
{
    public string UserId { get; set; }

    public AppUser User { get; set; }
    public Order Order { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; }
}
