
namespace BSYS.Application.Abstractions.Services;

public interface IActiveAdminService 
{
    public void AddActiveAdmin(string connectionId, string userId);
    public void RemoveInactiveAdmin(string connectionId);
    public bool IsAdminActive(string userId);
}
