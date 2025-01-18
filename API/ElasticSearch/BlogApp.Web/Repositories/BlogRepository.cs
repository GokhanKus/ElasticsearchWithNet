using BlogApp.Web.Models;
using Elastic.Clients.Elasticsearch;

namespace BlogApp.Web.Repositories
{
	public class BlogRepository(ElasticsearchClient _elasticClient)
	{
		private const string indexName = "blog";
		public async Task<Blog?> SaveChangesAsync(Blog newBlog)
		{
			newBlog.Created = DateTime.Now;

			var response = await _elasticClient.IndexAsync(newBlog, x => x.Index(indexName));//elsearch'te Indexlemek = Savechanges
																							 //.Id(Guid.NewGuid().ToString()) diyerek id biz de atayabiliriz ya da bu islem elasticsearch'e birakilabilir
			if (!response.IsValidResponse) return null;

			newBlog.Id = response.Id;
			return newBlog;
		}
		public async Task<List<Blog>> SearchAsync(string searchText)
		{
			//title'a veya content'e gore arama yap
			var results = await _elasticClient.SearchAsync<Blog>(s => s.Index(indexName)
				.Size(1000).Query(q => q
					.Bool(b => b
						.Should(
							s => s
								.MatchBoolPrefix(m => m
									.Field(f => f.Title)
									.Query(searchText)),
							s => s.Match(m => m
									.Field(f => f.Content)
									.Query(searchText))))));

			foreach (var blog in results.Hits)
				blog.Source.Id = blog.Id!;

			return results.Documents.ToList();
		}
	}
}
