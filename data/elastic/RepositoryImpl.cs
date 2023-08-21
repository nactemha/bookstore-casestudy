using Nest;
namespace ecommerce.data.elastic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IElasticClient _client;
        protected readonly string _indexName;

        public Repository(string connectionString, string indexName)
        {
            var settings = new ConnectionSettings(new Uri(connectionString)).DefaultIndex(indexName);
            _client = new ElasticClient(settings);
            _indexName = indexName;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var response = await _client.SearchAsync<T>(s => s.MatchAll());
            return response.Documents;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync<T>(id);
            return response.Source;
        }

        public async Task AddAsync(T entity)
        {
            await _client.IndexDocumentAsync(entity);
        }

        public void Update(T entity)
        {
            _client.Index(entity, idx => idx.Index(_indexName));
        }

        public void Delete(int Id)
        {
            _client.Delete<T>(Id);
        }
    

        public async Task SaveChangesAsync()
        {
            // In Elasticsearch with NEST, the operations like Add, Update, and Delete are directly persisted.
            // So, this operation may be a no-op in this context, but you can potentially use it for bulk updates or refresh index operations.
            await Task.CompletedTask;
        }
    }
}