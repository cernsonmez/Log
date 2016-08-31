using Log.DataTypes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Log.DataGeneration
{
    public interface IDatabaseConnection
    {
        void SaveDocument(LogInfo logMessage);
    }

    public class MongoConnection : IDatabaseConnection
    {
        private MongoClient _client;
        private IMongoCollection<LogInfo> _collection;
        public MongoConnection(string connector, string databaseName, string colName)
        {
            _client = new MongoClient(@connector);
            var database = _client.GetDatabase(databaseName);
            _collection = database.GetCollection<LogInfo>(colName);
        }
        public void SaveDocument(LogInfo logMessage)
        {
            _collection.InsertOne(logMessage);

        }
        public void Querys()
        {
            /////////////////////////////////////////////////////sorgu işlemleri
            var retrievedDocument = _collection.Find(new BsonDocument()).FirstOrDefault();

            //Veritabani kayıt sayısı
            var count = _collection.Count(new BsonDocument());
            Console.WriteLine(count);

            //Filtreleme yöneticisi oluşturma:
            var filterBuilder = Builders<LogInfo>.Filter;

            //Sorgulama:
            var filter = filterBuilder.Eq("appname", "SFS") & filterBuilder.Eq("sequence", 12);
            var retrievedDoc = _collection.Find(filter).ToList();

            //tarih sorgulama:
            var dateFilter = filterBuilder.Gte("timestamp", new DateTime(2016, 8, 1));

            //filtre uygulama:
            //var retrievedDoc = collection.Find(dateFilter).ToList();
        }

    }
}
