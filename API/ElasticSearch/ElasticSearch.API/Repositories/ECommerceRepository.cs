using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
	public class ECommerceRepository(ElasticsearchClient _elasticClient)
	{
		private const string indexName = "kibana_sample_data_ecommerce";
		public async Task<ImmutableList<ECommerce>> TermQuery(string customerFirstName)
		{
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
				.Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName)))); //.Size(10) default

			foreach (var hit in results.Hits)
				hit.Source.Id = hit.Id!;

			return results.Documents.ToImmutableList();
		}
	}
}
