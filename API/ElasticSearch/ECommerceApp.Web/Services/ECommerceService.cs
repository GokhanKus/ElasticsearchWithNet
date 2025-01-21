using ECommerceApp.Web.Repositories;
using ECommerceApp.Web.ViewModel;

namespace ECommerceApp.Web.Services
{
	public class ECommerceService(ECommerceRepository _eCommerceRepository)
	{
		public async Task<(List<ECommerceViewModel>, long totalItemCount, long pageLinkCount)> SearchAsync
			(ECommerceSearchViewModel eCommerceSearchViewModel, int page, int pageSize)
		{
			var (eCommerceList, totalItemCount) = await _eCommerceRepository.SearchAsync(eCommerceSearchViewModel, page, pageSize);

			//örneğin donen sorgu sonucu totalItemCount 65 olsun pagesize 10 olsun, pagelink count 7 olmalı yani 1 2 3 4 5 6 7 sayfaları olmalı onun matematigi yapildi
			var pageLinkCount = (totalItemCount / pageSize);
			if (totalItemCount % pageSize != 0) pageLinkCount += 1;

			var eCommerceViewModel = eCommerceList.Select(eCommerce => new ECommerceViewModel
			{
				Category = string.Join(",", eCommerce.Category),
				CustomerFullName = eCommerce.CustomerFullName,
				CustomerFirstName = eCommerce.CustomerFirstName,
				CustomerLastName = eCommerce.CustomerLastName,
				Gender = eCommerce.Gender.ToLower(),
				Id = eCommerce.Id,
				OrderId = eCommerce.OrderId,
				OrderDate = eCommerce.OrderDate.ToShortDateString(),
				TaxfulTotalPrice = eCommerce.TaxfulTotalPrice
			}).ToList();

			return (eCommerceViewModel, totalItemCount, pageLinkCount);
		}
	}
}
