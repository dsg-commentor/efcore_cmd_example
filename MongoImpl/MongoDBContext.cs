using System;
using System.Linq;
using System.Collections.Generic;
using Data.Entities;
using MongoDB.Driver;
using ServiceInterfaces;
using MongoDB.Bson.Serialization;

namespace MongoImpl
{
    public class MongoDBContext : IDataProvider
    {
        private static string _connectionString;
        public static string DatabaseName { get; set; }

        private IMongoDatabase _database { get; }

        public MongoDBContext(string connectionString, bool isSSL)
        {
            _connectionString = connectionString;
            BsonClassMap.RegisterClassMap<Employee>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            try
            {
                Console.WriteLine(connectionString);
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
                if (isSSL)
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                
                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access db server.", ex);
            }
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            var result = _database.GetCollection<Employee>("employees");
            return (result.Find(_ => true)).ToList();
        }

        public Employee AddEmployee(Employee employee)
        {
            _database.GetCollection<Employee>("employees").InsertOne(employee);
            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            return _database.GetCollection<Employee>("employees").DeleteOne(e => e.EmployeeId == id).IsAcknowledged;
        }
    }
}
