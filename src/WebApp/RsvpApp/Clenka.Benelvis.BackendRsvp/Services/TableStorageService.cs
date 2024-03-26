using Azure;
using Azure.Data.Tables;
using Clenka.Benelvis.BackendRsvp.Constants;
using Clenka.Benelvis.BackendRsvp.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity
    {
        private readonly TableClient _tableClient;
        private readonly IConfiguration _configuration;
        private readonly string _partitionKey = GlobalConstants.RSVPENTITYTABLEPARTITIONKEY;
        private readonly ILogger<TableStorageService<T>> _logger;

        public TableStorageService(IConfiguration configuration, ILogger<TableStorageService<T>> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _tableClient = GetTableClientAsync(GlobalConstants.RSVPENTITYTABLENAME).Result;

        }

        private async Task<TableClient> GetTableClientAsync(string tableName)
        {
            var x = GlobalConstants.RSVPENTITYCONNECTIONSTRINGNAME;
            var connectionString = _configuration.GetConnectionString(GlobalConstants.RSVPENTITYCONNECTIONSTRINGNAME);
            var tableServiceClient = new TableServiceClient(connectionString);
            var tableClient = tableServiceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        //public TableStorageService(TableServiceClient tableServiceClient, string tableName)
        //{
        //    _configuration = config;
        //    _tableClient = tableServiceClient.GetTableClient(tableName);
        //}

        public async Task<Azure.Response> AddAsync(T entity)
        {
            //var retVal = await _tableClient.UpsertEntityAsync<T>(entity);
            var retVal =  await _tableClient.AddEntityAsync<T>(entity);

            return retVal;
        }

        public async Task<int> DeleteASync(string id)
        {
            var res = await _tableClient.DeleteEntityAsync(_partitionKey, id);
            return res.Status;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            _logger.LogInformation("GetAllAsync");
            List<T> values = new List<T>();
            var page = _tableClient.QueryAsync<T>(x => x.PartitionKey == _partitionKey);
            
            await foreach (var item in page)
            {
                values.Add(item);
            }
            return values;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            List<T> values = new List<T>();
            var response = _tableClient.QueryAsync<T>(predicate);
            await foreach (var item in response)
            {
                values.Add(item);
            }
            return values;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            // Get single entity
            var response = await _tableClient.GetEntityAsync<T>(_partitionKey, id);
            if (response.HasValue)
            {
                return response.Value;
            }
            return response?.Value;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            int retvalue = 0;
            var response = await _tableClient.GetEntityAsync<T>(_partitionKey, entity.RowKey);
            if (response.HasValue)
            {
                var returnedItem = response.Value;
                var ret = await _tableClient.UpdateEntityAsync<T>(entity, ETag.All);//If you don’t care about the concurrent updates and want to force the update then you can pass Etag.All as the second argument.
                if (ret.Status == 204)
                {
                    retvalue = 1;
                }
            }
            return retvalue;
        }
    }
}
