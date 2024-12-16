using BLL.DTO;
using BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services
{
    public class SincService : ISincService
    {
        private readonly string _documentConnectionString;
        private readonly string _relativeConnectionString;
        private readonly IServiceProvider _documentProvider, _relativeProvider; 
        public SincService(string relativeConnectionString, string documentConnectionString)
        {
            _documentConnectionString = documentConnectionString;
            _relativeConnectionString = relativeConnectionString;

            var documentService = new ServiceCollection();
            var relativeService = new ServiceCollection();

            documentService.ConfigureBLL(documentConnectionString, true);
            relativeService.ConfigureBLL(relativeConnectionString);

            _documentProvider = documentService.BuildServiceProvider();
            _relativeProvider = relativeService.BuildServiceProvider();
        }

        public void SinkDBToMongo()
        {
            var clientDocumentService = _documentProvider.GetService<ClientService>();
            _relativeProvider.GetService<ClientService>().GetAll()
                .Intersect(clientDocumentService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(clientDocumentService.Create);

            var creditCardDocumentService = _documentProvider.GetService<CreditCardService>();
            _relativeProvider.GetService<CreditCardService>().GetAll()
                .Intersect(creditCardDocumentService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(creditCardDocumentService.Create);

            var orderDocumentService = _documentProvider.GetService<OrderService>();
            _relativeProvider.GetService<OrderService>().GetAll()
                .Intersect(orderDocumentService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(orderDocumentService.Create);

            var routeDocumentService = _documentProvider.GetService<RouteService>();
            _relativeProvider.GetService<RouteService>().GetAll() 
                .Intersect(routeDocumentService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(routeDocumentService.Create);

            var ticketDocumentService = _documentProvider.GetService<TicketService>();
            _relativeProvider.GetService<TicketService>().GetAll()
                .Intersect(ticketDocumentService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(ticketDocumentService.Create);
        }

        public void SinkMongoToDB()
        {
            var clientRelativeService = _relativeProvider.GetService<ClientService>();
            _documentProvider.GetService<ClientService>().GetAll()
                .Except(clientRelativeService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(clientRelativeService.Create);

            var creditCardRelativeService = _relativeProvider.GetService<CreditCardService>();
            _documentProvider.GetService<CreditCardService>().GetAll()
                .Except(creditCardRelativeService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(creditCardRelativeService.Create);

            var orderRelativeService = _relativeProvider.GetService<OrderService>();
            _documentProvider.GetService<OrderService>().GetAll()
                .Except(orderRelativeService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(orderRelativeService.Create);

            var routeRelativeService = _relativeProvider.GetService<RouteService>();
            _documentProvider.GetService<RouteService>().GetAll()
                .Except(routeRelativeService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(routeRelativeService.Create);

            var ticketRelativeService = _relativeProvider.GetService<TicketService>();
            _documentProvider.GetService<TicketService>().GetAll()
                .Except(ticketRelativeService.GetAll())
                .Distinct()
                .ToList()
                .ForEach(ticketRelativeService.Create);
        }
    }
}
