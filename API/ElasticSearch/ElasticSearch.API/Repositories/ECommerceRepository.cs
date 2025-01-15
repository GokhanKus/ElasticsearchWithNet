using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
	public class ECommerceRepository(ElasticsearchClient _elasticClient)
	{
		private const string indexName = "kibana_sample_data_ecommerce";
		public async Task<ImmutableList<ECommerce>> TermQueryAsync(string customerFirstName)
		{
			#region 1st way, 3rd way
			//1st way
			//var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
			//	.Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName)))); //.Size(10) default

			//3rd way
			//var termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };
			//var result = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery));
			#endregion
			//2nd way (type safe)
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
			.Query(q => q.Term(t => t.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerFirstName))));

			FillIdFields(results);

			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList)
		{
			List<FieldValue> terms = new List<FieldValue>();

			customerFirstNameList.ForEach(x =>
			{
				terms.Add(x);
			});

			//1st way
			//var termQuery = new TermsQuery()
			//{
			//	Field = "customer_first_name.keyword",
			//	Terms = new TermsQueryField(terms.AsReadOnly())
			//};
			//var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery).Size(30));

			//2nd way
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName).Size(50)
			.Query(q => q.Terms(t => t
			.Field(f => f.CustomerFirstName.Suffix("keyword"))
			.Terms(new TermsQueryField(terms.AsReadOnly())))));

			FillIdFields(results);

			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullNamePrefix)
		{
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
			.Query(q => q
			.Prefix(p => p
			.Field(f => f.CustomerFullName
			.Suffix("keyword"))
			.Value(customerFullNamePrefix))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double minTaxfulTotalPrice, double maxTaxfulTotalPrice)
		{
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName).Size(50)
			.Query(q => q
			.Range(r => r
			.NumberRange(nr => nr
			.Field(f => f.TaxfulTotalPrice)
			.Gte(minTaxfulTotalPrice)
			.Lte(maxTaxfulTotalPrice)))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync()
		{
			//butun datalar
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s
				.Index(indexName)
				.Query(q => q.MatchAll(_ => { })));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> PaginationQueryAsync(int page, int pageSize)
		{
			var pageFrom = (page - 1) * pageSize;

			var results = await _elasticClient.SearchAsync<ECommerce>(s => s
				.Index(indexName).From(pageFrom).Size(pageSize)
				.Query(q => q.MatchAll(_ => { })));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFirstName)
		{
			//ornegin Ed?ie yazinca Eddie olanlari getirir
			//ya da E*ie yazarsak Eddie olanlari getirir
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
				.Query(q => q.Wildcard(w =>w
				.Field(f => f.CustomerFirstName
				.Suffix("keyword"))
				.Wildcard(customerFirstName))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		private void FillIdFields(SearchResponse<ECommerce> results)
		{
			foreach (var hit in results.Hits)
				hit.Source.Id = hit.Id!;
		}
	}
}
