
namespace ecommerce.service
{
    public class Settings : ISettings
    {
        private readonly IConfigurationRoot configuration;
        public Settings(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public TimeSpan GetTokenExperation()
        {
            return TimeSpan.FromMinutes(int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"]));
        }

        public string GetSecretKey()
        {
            return configuration["JwtSettings:Key"];
        }

        public string PostgreConnectionString()
        {
            return configuration["Postgre:ConnectionString"];
        }
        public string ElasticConnectionString()
        {
            return configuration["Elastic:ConnectionString"];
        }
        public string ElasticIndexName()
        {
            return configuration["Elastic:IndexName"];
        }


    }

}