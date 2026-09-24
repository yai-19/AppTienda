using AppTienda.Infrastructure.Shared.Interfaces;
using AppTienda.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppTienda.Infrastructure.Shared
{
    public static class ServicesRegistration
    {
        public static void AddSharedLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IUploadFileService, UploadFileService>();
        }
    }
}
