using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.Models;

namespace ElasticSearch.API.Repositories
{
	//bu projenin amaci elastic search uzerine yogunlasmak oldugu icin ntier yapmaya gerek yok
	public class ProductRepository(ElasticsearchClient _elasticClient)//primary ctor DI
	{
		public async Task<Product?> SaveChangesAsync(Product newProduct)
		{
			newProduct.Created = DateTime.Now;

			var response = await _elasticClient.IndexAsync(newProduct, x => x.Index("products"));//elsearch'te Indexlemek = Savechanges
			//.Id(Guid.NewGuid().ToString()) diyerek id biz de atayabiliriz ya da bu islem elasticsearch'e birakilabilir

			if (!response.IsValidResponse) return null; //kayit islemi basarili olmazsa,, !IsSuccess() ile hata da firlatilabilir..

			newProduct.Id = response.Id;
			return newProduct;
		}
	}
}
