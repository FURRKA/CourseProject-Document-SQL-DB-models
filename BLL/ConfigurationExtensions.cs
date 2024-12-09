using BLL.DTO;
using BLL.Interfaces;
using BLL.Profiles;
using BLL.Services;
using DAL;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureBLL(this IServiceCollection service, string connection, bool isDocument = false)
        {
            service.ConfigureDAL(connection, isDocument);
            service.AddAutoMapper(
                typeof(ClientProfile),
                typeof(CreditCardProfile),
                typeof(DistancesProfile),
                typeof(OrderProfile),
                typeof(RouteProfile),
                typeof(StationProfile),
                typeof(StationRouteProfile),
                typeof(TicketOrderProfile),
                typeof(TicketProfile)
            );

            service.AddTransient<IService<ClientDTO>, ClientService>();
        }
    }
}
