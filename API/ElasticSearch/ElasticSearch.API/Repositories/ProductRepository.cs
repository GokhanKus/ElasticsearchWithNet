using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.DTOs;
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

			if (!response.IsSuccess()) return null; //kayit islemi basarili olmazsa,, !IsSuccess() ile hata da firlatilabilir..

			newProduct.Id = response.Id;
			return newProduct;
		}
		public async Task<bool> UpdateAsync(ProductUpdateDto productDto)
		{
			var response = await _elasticClient.UpdateAsync<Product, ProductUpdateDto>(productDto.Id, s => s.Index(indexName).Doc(productDto));

			return response.IsSuccess();
		}

		public async Task<DeleteResponse> DeleteAsync(string id)
		{
			var response = await _elasticClient.DeleteAsync<Product>(id, s => s.Index(indexName));

			return response;
		}
		public async Task<ImmutableList<Product>> GetAllAsync()
		{
			var response = await _elasticClient.SearchAsync<Product>(s => s.Index(indexName).Query(new MatchAllQuery())); //From().Size() belirtilebilir

			//metadatadan gelen idleri bizim id alanimiza mapliyoruz
			foreach (var hit in response.Hits)
				hit.Source.Id = hit.Id!;

			return response.Documents.ToImmutableList();
		}
		public async Task<Product?> GetByIdAsync(string id)
		{
			var response = await _elasticClient.GetAsync<Product>(id, s => s.Index(indexName));

			if (!response.IsSuccess())
				return null;

			response.Source.Id = response.Id;

			return response.Source;
		}
	}
}
