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
	}
}
