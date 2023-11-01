using BSYS.Application.Abstractions.Services.Configurations;
using BSYS.Domain.Entities;
using BSYS.Domain.Entities.Identity;
using BSYS.Application.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BSYS.Application.Abstractions.UoW;

namespace BSYS.Persistence.Services;

public class AuthorizationEndpointService : IAuthorizationEndpointService
{
    readonly IApplicationService _applicationService;
    readonly RoleManager<AppRole> _roleManager;
    private readonly IUnitofWork _uow;
    public AuthorizationEndpointService(IApplicationService applicationService,
        IUnitofWork uow,
        RoleManager<AppRole> roleManager)
    {
        _applicationService = applicationService;
        _roleManager = roleManager;
        _uow = uow;
    }

    public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
    {
        Menu _menu = await _uow.MenuReadRepository.GetSingleAsync(m => m.Name == menu);
        if (_menu == null)
        {
            _menu = new()
            {
                Id = Guid.NewGuid(),
                Name = menu
            };
            await _uow.MenuWriteRepository.AddAsync(_menu);

            await _uow.MenuWriteRepository.SaveAsync();
        }

        Endpoint? endpoint = await _uow.EndpointReadRepository.Table.Include(e => e.Menu).Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);

        if (endpoint == null)
        {
            var action = _applicationService.GetAuthorizeDefinitionEndpoints(type)
                    .FirstOrDefault(m => m.Name == menu)
                    ?.Actions.FirstOrDefault(e => e.Code == code);

            endpoint = new()
            {
                Code = action.Code,
                ActionType = action.ActionType,
                HttpType = action.HttpType,
                Definition = action.Definition,
                Id = Guid.NewGuid(),
                Menu = _menu
            };

            await _uow.EndpointWriteRepository.AddAsync(endpoint);
            await _uow.EndpointWriteRepository.SaveAsync();
        }

        foreach (var role in endpoint.Roles)
            endpoint.Roles.Remove(role);

        var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

        foreach (var role in appRoles)
            endpoint.Roles.Add(role);

        await _uow.EndpointWriteRepository.SaveAsync();
    }

    public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
    {
        Endpoint? endpoint = await _uow.EndpointReadRepository.Table
            .Include(e => e.Roles)
            .Include(e => e.Menu)
            .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
        if (endpoint != null)
            return endpoint.Roles.Select(r => r.Name).ToList();
        return null;
    }
}
