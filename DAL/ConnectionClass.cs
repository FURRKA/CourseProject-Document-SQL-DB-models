using DAL.Interfaces;

namespace DAL
{
    internal class ConnectionClass : IConnectionString
    {
        public string ConnectionString { get; set; }
        public ConnectionClass(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
