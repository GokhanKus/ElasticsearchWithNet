namespace BlogApp.Web.ViewModel
{
	//bu model içerisinde (SearchPageViewModel) sol taraftaki filteleme barlari (ECommerceSearchViewModel), datalar (List<ECommerceViewModel) ve pagination var
	public class SearchPageViewModel
	{
		public long TotalItemCount { get; set; }
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public long PageLinkCount { get; set; }
		public List<ECommerceViewModel>? List { get; set; }
		public ECommerceSearchViewModel SearchViewModel { get; set; }

		//ornegin toplam 50 sayfamız olsun client 20.sayfaya tıklarsa 14 15 16 17 18 19 20 21 22 23 24 25 26 sayfalarını paginationda gosterecek
		//cunku 1den 50 ye kadar butun sayfaları gostermesini istemiyoruz
		public int StartPage() => Page - 6 <= 0 ? 1 : Page - 6;
		public long EndPage() => Page + 6 >= PageLinkCount ? PageLinkCount : Page + 6;

		public string CreatePageUrl(HttpRequest request, long page, int pageSize)
		{
			var currentUrl = new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}").AbsoluteUri;

			if (currentUrl.Contains("page", StringComparison.OrdinalIgnoreCase))
			{
				currentUrl = currentUrl.Replace($"Page={Page}", $"Page={page}", StringComparison.OrdinalIgnoreCase);
				currentUrl = currentUrl.Replace($"PageSize={PageSize}", $"Page={pageSize}", StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				currentUrl = $"{currentUrl}?Page={page}";
				currentUrl = $"{currentUrl}&PageSize={pageSize}";
			}
			return currentUrl;
		}
	}
}
