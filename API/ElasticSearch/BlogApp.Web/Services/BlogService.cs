﻿using BlogApp.Web.Models;
using BlogApp.Web.Repositories;
using BlogApp.Web.ViewModel;

namespace BlogApp.Web.Services
{
	public class BlogService(BlogRepository _blogRepository)
	{
		public async Task<bool> SaveIndex(BlogCreateViewModel blogCreateModel)
		{
			var blog = new Blog
			{
				Title = blogCreateModel.Title,
				Content = blogCreateModel.Content,
				Tags = blogCreateModel.Tags.ToArray(),
				UserId = Guid.NewGuid(),
			};
			var isCreatedBlog = await _blogRepository.SaveChangesAsync(blog);
			return isCreatedBlog != null;
		}
	}
}
