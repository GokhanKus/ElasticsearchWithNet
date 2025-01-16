using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models;
using ElasticSearch.API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
	public class ECommerceRepository(ElasticsearchClient _elasticClient)
	{
		private const string indexName = "kibana_sample_data_ecommerce";

		//TERM LEVEL QUERIES
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
		public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerFullName)
		{
			//clientten gelen 2 harf hatasina kadar tolere edebilir
			//ornegin Olivre Lov yazdigimizda Oliver Love kisisi gelir
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
				.Query(q => q.Fuzzy(fu =>fu
				.Field(f => f.CustomerFullName.Suffix("keyword")).Value(customerFullName)
				.Fuzziness(new Fuzziness(2))))
				.Sort(sort => sort
				.Field(f => f.TaxfulTotalPrice, new FieldSort() { Order = SortOrder.Desc })));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}

		//FULL-TEXT QUERIES
		public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName)
		{
			//orn Men's Clothing (no matter case sensitive) yazdigimizda operator and ise icerisinde "Men's "Clothing" geçen 
			//kategorileri getirir operator or olsaydı "Men's" ya da "Clothing" gecenleri getirecekti 
			var results = await _elasticClient.SearchAsync<ECommerce>(s=>s.Index(indexName)
			.Query(q=>q
			.Match(m=>m
			.Field(f=>f.Category)
			.Query(categoryName).Operator(Operator.And))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string customerFullName)
		{
			//üstteki Match queryden farklı olarak burada tam bir kelimeyi yazmak zorunda değiliz ilk harflerini yazarsak onla başlayanları getirir. orn
			//sorgu olarak "abiga ha" yazarsam sonuc olarak Abigail Hampton, Kamal Hale, Yasmine Haynes gibi isimleri getirir
			var results = await _elasticClient.SearchAsync<ECommerce>(s=>s.Index(indexName)
			.Query(q=>q
			.MatchBoolPrefix(m=>m
			.Field(f=>f.CustomerFullName)
			.Query(customerFullName))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> MatchPhraseQueryFullTextAsync(string customerFullName)
		{
			//orn. sultan al yazdığımda fullname kısmında "sultan al"'i bir bütün olarak alıp icinde bu ifade gecenleri getirecek
			var results = await _elasticClient.SearchAsync<ECommerce>(s=>s.Index(indexName)
			.Query(q=>q
			.MatchPhrase(m=>m
			.Field(f=>f.CustomerFullName)
			.Query(customerFullName))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> MatchPhrasePrefixQueryFullTextAsync(string customerFullName)
		{
			//Autocomplete (tamamlama) özelliği için kullanılır. Cümlenin başlangıcına göre arama yapar.
			//orn sonya sm yazdığımda sonya smith gelir
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
			.Query(q => q
			.MatchPhrasePrefix(m => m
			.Field(f => f.CustomerFullName)
			.Query(customerFullName))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> MatchPhrasePrefixQueryByProductNameFullTextAsync(string productName)
		{
			var results = await _elasticClient.SearchAsync<ECommerce>(s=>s.Index(indexName)
			.Query(q=>q
			.MatchPhrasePrefix(m=>m
			.Field("products.product_name")
			.Query(productName))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}
		public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName, double taxfulTotalPrice, string categoryName, string manufacturer)
		{
			//cityname: New York olan, taxfulTotalPrice:100'den küçük olmayan,  categoryName: Women's clothing olan, manufacturer: Tigress Enterprises olan kayitlari getir.
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
				.Size(1000).Query(q => q
					.Bool(b => b
						.Must(m => m
							.Term(t => t
							.Field("geoip.city_name")
							.Value(cityName)))
						.MustNot(mn => mn
							.Range(r => r
							.NumberRange(nr => nr
							.Field(f => f.TaxfulTotalPrice)
							.Lte(taxfulTotalPrice))))
						.Should(s => s.Term(t => t
							.Field(f => f.Category.Suffix("keyword"))
							.Value(categoryName)))
						.Filter(f => f
							.Term(t => t
							.Field("manufacturer.keyword")
							.Value(manufacturer))))));

			FillIdFields(results);
			return results.Documents.ToImmutableList();
		}

		public async Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoAsync(string customerFullName)
		{
			//ya match query ya da prefix query saglanacak 
			//orn. ya tam eşleşecek ya da o sorgulanan harflerle başlayan veriler gelecek

			//1st way
			//var result = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
			//	.Size(1000).Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

			//2nd way
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(indexName)
				.Size(1000).Query(q => q
					.Bool(b => b
						.Should(m => m
							.Match(m => m
								.Field(f => f.CustomerFullName)
								.Query(customerFullName))
							.Prefix(p => p
								.Field(f => f.CustomerFullName.Suffix("keyword"))
								.Value(customerFullName))))));
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
