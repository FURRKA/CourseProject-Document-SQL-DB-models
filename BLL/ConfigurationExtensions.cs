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
            service.AddTransient<IService<CreditCardDTO>, CreditCardService>();
            service.AddTransient<IService<DistancesDTO>, DistanceService>();
            service.AddTransient<IService<OrderDTO>, OrderService>();
            service.AddTransient<IService<RouteDTO>, RouteService>();
            service.AddTransient<IService<StationDTO>, StationService>();
            service.AddTransient<IService<StationRoutesDTO>, StationRoutsService>();
            service.AddTransient<IService<TicketDTO>, TicketService>();
            service.AddTransient<IService<TicketOrdersDTO>, TicketOrdersService>();

            service.AddTransient<IAutorizationService, AuthorizationService>();
        }
    }
}
