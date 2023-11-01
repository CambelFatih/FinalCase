using BSYS.Application.DTOs.Configuration;

namespace BSYS.Application.Abstractions.Services.Configurations;

public interface IApplicationService
{
    List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
}
