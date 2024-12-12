using BLL.DTO;
using BLL.Interfaces;

namespace BLL.Services
{
    public class AuthorizationService : IAutorizationService
    {
        private readonly IService<ClientDTO> _clientService;
        public AuthorizationService(IService<ClientDTO> service)
        { 
            _clientService = service;
        }
        public ClientDTO? Autorization(string login, string password)
        {
            var data = _clientService.GetByCriteria(c => c.Login == login && c.Password == password);
            return data.Count > 0? data[0] : null;
        }

        public bool DeleteAccount()
        {
            Console.WriteLine("Вы точно хотите удалить свой аккаунт?\nДля поддтверждения нажмите Y");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Y)
            {
                Console.WriteLine("\nДля поддтверждения удаления введите свой логин и пароль");
                Console.Write("Логин: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                var account = _clientService.GetByCriteria(c => c.Login == login && c.Password == password)[0];
                if (account != null)
                {
                    _clientService.Delete(account);
                    Console.WriteLine("Ваша учётная запись успешно удалена.\nЖаль мы не смоги оправдать ваших ожиданий :с");
                    Console.ReadKey();
                    return true;
                }
                else
                {
                    Console.WriteLine("Логин или пароль не совпали. Мы не можем удалить вашу учётную запись без подтверждения, что это вы.");
                    return false;
                }
            }
            else
                return false;

        }

        public bool Registrating()
        {
            string name, lastName, surName, login, password;
            Console.WriteLine("Пожалуйста введите ваши данные для регистрации в системе");

            Console.WriteLine("Введите ваше Имя");
            name = Console.ReadLine();

            Console.WriteLine("Введите вашу Фамилию ");
            lastName = Console.ReadLine();

            Console.WriteLine("Введите ваше Отчество");
            surName = Console.ReadLine();

            var clientdata = _clientService.GetAll();
            while (true)
            {
                Console.WriteLine("Введите желаемый логин");
                login = Console.ReadLine();
                if (clientdata.Any(c => c.Login == login))
                    Console.WriteLine("К сожалению данный логин уже занят другим пользователем. Попробуйте использовать другой логин\n\n");
                else
                    break;
            }

            Console.WriteLine("Введите желаемый пароль ");
            password = Console.ReadLine();

            var newClient = new ClientDTO(name, lastName, surName, login, password);
            _clientService.Create(newClient);
            Console.WriteLine("Регистрация успешно завершена!");
            Console.ReadKey();
            return true;
        }
    }
}
