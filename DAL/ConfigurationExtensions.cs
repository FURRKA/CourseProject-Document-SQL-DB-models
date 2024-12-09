using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DAL
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureDAL(this IServiceCollection service, string connection, bool isDocument = false)
        {
            var connectionObject = new ConnectionClass(connection);
            service.AddTransient<IConnectionString>(_ => connectionObject);

            if (isDocument)
            {
                var mongoClient = new MongoClient(connection);
                IMongoDatabase database = mongoClient.GetDatabase("TrainTickets");

                service.AddTransient<IRepository<ClientsEntity>>(_ => new MongoRepository<ClientsEntity>(database, connectionObject));
                service.AddTransient<IRepository<TicketEntity>>(_ => new MongoRepository<TicketEntity>(database, connectionObject));
                service.AddTransient<IRepository<CreditsCard>>(_ => new MongoRepository<CreditsCard>(database, connectionObject));
                service.AddTransient<IRepository<Distances>>(_ => new MongoRepository<Distances>(database, connectionObject));
                service.AddTransient<IRepository<StationsEntity>>(_ => new MongoRepository<StationsEntity>(database, connectionObject));
                service.AddTransient<IRepository<RouteEntity>>(_ => new MongoRepository<RouteEntity>(database, connectionObject));
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
