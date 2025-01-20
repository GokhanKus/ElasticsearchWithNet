﻿using BlogApp.Web.Models;
using BlogApp.Web.ViewModel;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace BlogApp.Web.Repositories
{
	public class ECommerceRepository(ElasticsearchClient _elasticClient)
	{
		private const string IndexName = "kibana_sample_data_ecommerce";
		public async Task<(List<ECommerce>, long totalItemCount)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize)
		{
			List<Action<QueryDescriptor<ECommerce>>> queryLists = new();

			if (!string.IsNullOrEmpty(searchViewModel.Category))
			{
				queryLists.Add((q) => q.Match(m => m
									.Field(f => f.Category)
									.Query(searchViewModel.Category)));
			}
			if (!string.IsNullOrEmpty(searchViewModel.CustomerFullName))
			{
				queryLists.Add((q) => q.Match(m => m
									.Field(f => f.Category)
									.Query(searchViewModel.CustomerFullName)));
			}
			if (searchViewModel.OrderDateStart.HasValue)
			{
				queryLists.Add((q) => q.Range(r => r
									.DateRange(dr => dr
									.Field(f => f.OrderDate)
									.Gte(searchViewModel.OrderDateStart.Value))));
			}
			if (searchViewModel.OrderDateEnd.HasValue)
			{
				queryLists.Add((q) => q.Range(r => r
									.DateRange(dr => dr
									.Field(f => f.OrderDate)
									.Lte(searchViewModel.OrderDateEnd.Value))));
			}
			if (!string.IsNullOrEmpty(searchViewModel.Gender))
			{
				queryLists.Add((q) => q.Term(t => t
									.Field(f => f.Gender)
									.Value(searchViewModel.Gender)));
			}
			var pageFrom = (page - 1) * pageSize;

			//title'a veya content'e gore arama yap
			var results = await _elasticClient.SearchAsync<ECommerce>(s => s.Index(IndexName)
				.Size(pageSize).From(pageFrom)
				.Query(q => q
				.Bool(b => b
				.Must(queryLists.ToArray()))));

			foreach (var hit in results.Hits)
				hit.Source.Id = hit.Id!;

			return (results.Documents.ToList(), results.Total);
		}
	}
}
