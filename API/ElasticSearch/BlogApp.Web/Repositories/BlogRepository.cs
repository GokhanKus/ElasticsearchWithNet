using BlogApp.Web.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

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
			List<Action<QueryDescriptor<Blog>>> queryLists = new();

			Action<QueryDescriptor<Blog>> matchAllQuery = (q) => q.MatchAll(_ => { });

			Action<QueryDescriptor<Blog>> matchBoolPrefixByTitle = (q) => q.MatchBoolPrefix(m => m
									.Field(f => f.Title)
									.Query(searchText));

			Action<QueryDescriptor<Blog>> matchQueryByContent = (q) => q.Match(m => m
									.Field(f => f.Content)
									.Query(searchText));
			
			Action<QueryDescriptor<Blog>> termLevelQueryByTag = (q) => q.Term(m => m
									.Field(f => f.Tags)
									.Value(searchText));

			if (string.IsNullOrEmpty(searchText))
			{
				queryLists.Add(matchAllQuery);
			}
			else
			{
				queryLists.Add(matchBoolPrefixByTitle);
				queryLists.Add(matchQueryByContent);
				queryLists.Add(termLevelQueryByTag);
			}

			//title'a veya content'e gore arama yap
			var results = await _elasticClient.SearchAsync<Blog>(s => s.Index(indexName)
				.Size(1000).Query(q => q
					.Bool(b => b
						.Should(queryLists.ToArray()))));
			
			foreach (var blog in results.Hits)
				blog.Source.Id = blog.Id!;

			return results.Documents.ToList();
		}
	}
}
