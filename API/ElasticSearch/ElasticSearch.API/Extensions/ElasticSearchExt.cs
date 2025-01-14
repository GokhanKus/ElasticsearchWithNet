using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticSearch.API.Extensions
{
	public static class ElasticSearchExt
	{
		public static void AddElasticExt(this IServiceCollection services, IConfiguration configuration)
		{
			var userName = configuration.GetSection("Elastic")["Username"];
			var password = configuration.GetSection("Elastic")["Password"];
			var elasticUrl = configuration.GetSection("Elastic")["Url"];

			var pool = new SingleNodePool(new Uri(elasticUrl!));
			var settings = new ElasticsearchClientSettings(pool).Authentication(new BasicAuthentication(userName!,password!));

			var client = new ElasticsearchClient(settings);
			services.AddSingleton(client); //elastic firmasinin önerisi, ioc kaydinin singleton olarak yapilmasi..
		}
	}
}
