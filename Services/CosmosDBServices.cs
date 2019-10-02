namespace starter_dotnet_core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using starter_dotnet_core.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Azure.Cosmos;
    using starter_dotnet_core.Wheels;
    using System;
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        //Gets the models from the Cosmos database [MWH]
        public async Task<IEnumerable<Model>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Model>(new QueryDefinition(queryString));
            List<Model> results = new List<Model>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                
                results.AddRange(response.ToList());
            }

            return results;
        }

        //Gets a wheel from Cosmos
        public async Task<IEnumerable<CarWheel>> GetWheelItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<CarWheel>(new QueryDefinition(queryString));
            List<CarWheel> results = new List<CarWheel>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                
                results.AddRange(response.ToList());
            }

            return results;
        }

        //Adds a new model to the Cosmos database [MWH]
        //new PartitionKey(data.VehicleModel)
        public async Task<Model> AddItemASync(Model data){
            var response = await this._container.CreateItemAsync<Model>(data, new PartitionKey(data.VehicleModel));
            return response.Resource;

        }

        //Adds a new wheel to the Cosmos database [MWH]
        //new PartitionKey(data.VehicleModel)
        public async Task<CarWheel> AddWheelItemASync(CarWheel data){
            Console.Write(data.Model);
            var response = await this._container.CreateItemAsync<CarWheel>(data, new PartitionKey(data.WheelDimensions.Width));
            return response.Resource;

        }
    }
}