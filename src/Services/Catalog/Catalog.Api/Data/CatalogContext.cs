using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            string connString = "";
            string runningInContainer = System.Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
            if (!string.IsNullOrEmpty(runningInContainer))
            {
                if (bool.Parse(runningInContainer))
                    connString = System.Environment.GetEnvironmentVariable("DatabaseSetting:ConnectionString");
                else
                    connString = configuration.GetValue<string>("DatabaseSetting:ConnectionString");
            }
            
            if (string.IsNullOrEmpty(connString))
            {
                connString = configuration.GetValue<string>("DatabaseSetting:ConnectionString");
            }
           
            var client = new MongoClient(connString);
            var dataase=client.GetDatabase(configuration.GetValue<string>("DatabaseSetting:DatabaseName"));
            Products = dataase.GetCollection<Product>(configuration.GetValue<string>("DatabaseSetting:CollectionName"));
            CatalogContextSeed.SeedData(Products);

        }
         public IMongoCollection<Product> Products { get; }
    }
}
