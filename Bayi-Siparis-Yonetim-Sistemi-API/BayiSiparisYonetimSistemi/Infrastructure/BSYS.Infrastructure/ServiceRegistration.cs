using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.Services.Configurations;
using BSYS.Application.Abstractions.Storage;
using BSYS.Application.Abstractions.Token;
using BSYS.Infrastructure.Enums;
using BSYS.Infrastructure.Services;
using BSYS.Infrastructure.Services.Configurations;
using BSYS.Infrastructure.Services.Storage;
using BSYS.Infrastructure.Services.Storage.Azure;
using BSYS.Infrastructure.Services.Storage.Local;
using BSYS.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace BSYS.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IStorageService, StorageService>();
        serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        serviceCollection.AddScoped<IMailService, MailService>();
        serviceCollection.AddScoped<IApplicationService, ApplicationService>();
        serviceCollection.AddScoped<IQRCodeService, QRCodeService>();
    }
    public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
    {
        serviceCollection.AddScoped<IStorage, T>();
    }
    public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
    {
        switch (storageType)
        {
            case StorageType.Local:
                serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;
            case StorageType.Azure:
                serviceCollection.AddScoped<IStorage, AzureStorage>();
                break;
            case StorageType.AWS:

                break;
            default:
                serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;
        }
    }
}
