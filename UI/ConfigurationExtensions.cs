using BLL;
using Microsoft.Extensions.DependencyInjection;

namespace UI
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureUI(this IServiceCollection services, string connectionString, bool isDocument = false)
        {
            services.ConfigureBLL(connectionString, isDocument);
            services.AddTransient<UIService>();
        }
    }
}
