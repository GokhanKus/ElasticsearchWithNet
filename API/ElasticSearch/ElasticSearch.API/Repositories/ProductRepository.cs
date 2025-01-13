using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
	//bu projenin amaci elastic search uzerine yogunlasmak oldugu icin ntier yapmaya gerek yok
	public class ProductRepository(ElasticsearchClient _elasticClient)//primary ctor DI
	{
		private const string indexName = "products";
		public async Task<Product?> SaveChangesAsync(Product newProduct)
		{
			newProduct.Created = DateTime.Now;

			var response = await _elasticClient.IndexAsync(newProduct, x => x.Index(indexName));//elsearch'te Indexlemek = Savechanges
																								//.Id(Guid.NewGuid().ToString()) diyerek id biz de atayabiliriz ya da bu islem elasticsearch'e birakilabilir

			if (!response.IsValidResponse) return null; //kayit islemi basarili olmazsa,, !IsSuccess() ile hata da firlatilabilir..

			newProduct.Id = response.Id;
			return newProduct;
		}
		public async Task<ImmutableList<Product>> GetAllAsync()
		{
			var result = await _elasticClient.SearchAsync<Product>(s => s.Index(indexName).Query(new MatchAllQuery())); //From().Size() belirtilebilir

			foreach (var hit in result.Hits) 
				hit.Source.Id = hit.Id!;
		
			
			return result.Documents.ToImmutableList();
		}
	}
}
