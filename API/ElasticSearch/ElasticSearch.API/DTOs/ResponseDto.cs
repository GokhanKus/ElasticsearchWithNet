using System.Net;

namespace ElasticSearch.API.DTOs
{
	public record ResponseDto<T>
	{
		public T? Data { get; init; }
		public List<string>? Errors { get; init; }
		public HttpStatusCode StatusCode { get; init; }


		//factory design pattern - static Factory Method
		public static ResponseDto<T> Success(T? data, HttpStatusCode statusCode)
		{
			return new ResponseDto<T> { Data = data, StatusCode = statusCode };
		}
		public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode statusCode)
		{
			return new ResponseDto<T> { Errors = errors, StatusCode = statusCode };
		}
		public static ResponseDto<T> Fail(string error, HttpStatusCode statusCode)
		{
			return Fail(new List<string> { error }, statusCode);
		}
	}
}
