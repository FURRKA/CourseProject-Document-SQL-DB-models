using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new ServiceCollection();
            string connectionString;
            Console.WriteLine("Выберите базу данных с которой хотите взаимодействовать:\n1. PostgreSQL\n2. MongoDB");
            Console.Write("Ваш выбор: ");
            do
            {
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                        service.ConfigureUI(connectionString);
                        break;
                    case "2":
                        connectionString = ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
                        service.ConfigureUI(connectionString, true);
                        break;
                    default:
                        Console.WriteLine("Такого пункта нет. Попробуйте ещё раз");
                        break;
                }

                var provider = service.BuildServiceProvider();
                provider.GetService<UIService>().Run();
            } while (true);
        }
    }
}
