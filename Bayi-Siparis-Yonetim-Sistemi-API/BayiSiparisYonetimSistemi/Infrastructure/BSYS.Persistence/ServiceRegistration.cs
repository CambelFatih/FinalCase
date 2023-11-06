using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BSYS.Domain.Entities.Identity;
using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.Services.Authentications;
using BSYS.Persistence.Services;
using BSYS.Persistence.Contexts;
using BSYS.Application.Abstractions.UoW;
using BSYS.Persistence.UoW;

namespace BSYS.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<BSYSDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<BSYSDbContext>()
        .AddDefaultTokenProviders();

        
        services.AddScoped<IUnitofWork, UnitofWork>();

        services.AddSingleton<IActiveAdminService, ActiveAdminService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExternalAuthentication, AuthService>();
        services.AddScoped<IInternalAuthentication, AuthService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
    }
}
