namespace ecommerce.service
{
    public interface ISettings
    {
        TimeSpan GetTokenExperation();
        string GetSecretKey();
        string PostgreConnectionString();
        string ElasticConnectionString();
        string ElasticIndexName();
    }

}