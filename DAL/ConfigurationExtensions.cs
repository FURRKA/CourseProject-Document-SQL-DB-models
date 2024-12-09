using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureDAL(this IServiceCollection service, string connection, bool isDocument = false)
        {
            service.AddTransient<IConnectionString>(_ => new ConnectionClass(connection));

            if (isDocument)
            {
                //var mongoClient = new MongoClient(connection);
                //service.AddTransient<IRepository<ClientsEntity>, MongoRepository<ClientsEntity>>();
                //service.AddTransient<IMongoDatabase>(_ => mongoClient.GetDatabase("CourceWork"));
            }
            else
            {
                service.AddTransient<IRepository<ClientsEntity>, ClientsRepository>();
                service.AddTransient<IRepository<StationsEntity>, StationsRepository>();
                service.AddTransient<IRepository<StationRoutesEntity>, StationsRoutesRepository>();
                service.AddTransient<IRepository<Distances>, DistancesRepository>();

                service.AddTransient<IRepository<CreditsCard>, CardsRepository>();
                service.AddTransient<IFindByCardNumber, CardsRepository>();

                service.AddTransient<IRepository<OrderEntity>, OrderRepository>();

                service.AddTransient<IRepository<TicketOrdersEntity>, TicketsOrdersRepository>();
                service.AddTransient<IRepository<TicketEntity>,TicketRepository>();
                service.AddTransient<IRepository<RouteEntity>,RoutesRepository>();

                service.AddTransient<CardsRepository>();
                service.AddTransient<ClientsRepository>();
                service.AddTransient<DistancesRepository>();
                service.AddTransient<OrderRepository>();
                service.AddTransient<RoutesRepository>();
                service.AddTransient<StationsRepository>();
                service.AddTransient<StationsRoutesRepository>();
                service.AddTransient<TicketRepository>();
                service.AddTransient<TicketsOrdersRepository>();
                service.AddTransient<CardsRepository>();
            }
        }
    }
}
