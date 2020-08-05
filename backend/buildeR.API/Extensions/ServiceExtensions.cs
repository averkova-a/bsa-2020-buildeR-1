using buildeR.BLL.Interfaces;
using buildeR.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace buildeR.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}