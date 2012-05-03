using MongoDB.Driver;

namespace Punch.Data
{
    public static class ConnectionStrings
    {
        public static string DataBaseName
        {
            get { return "punch"; }
        }
        public static string ConnectionString
        {
            get { return "mongodb://localhost"; }
        }
    }
    
    
    public static class ConnectionManager<T>
    {
       
        
        private static MongoDatabase _connection;
        private static MongoDatabase Connection
        {
            get { return _connection ?? (_connection = GetDb()); }
        }

        private static MongoDatabase GetDb()
        {
            var server = MongoServer.Create(ConnectionStrings.ConnectionString);
            return server.GetDatabase(ConnectionStrings.DataBaseName);
        }

        public static MongoCollection<T> GetCollection(string collectionName)
        {
            return Connection.GetCollection<T>(collectionName);
        }
    }
}