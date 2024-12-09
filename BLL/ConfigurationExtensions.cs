using DAL;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureBLL(this IServiceCollection service, string connection, bool isDocument = false)
        {
            service.ConfigureDAL(connection, isDocument);
        }
    }
}
