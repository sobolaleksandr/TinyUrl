using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Services;

public interface IUrlService
{
	Task<UrlModel> CreateUrl(string url);

	Task<UrlModel?> GetByFullUrl(string url);

	Task<UrlModel?> GetByShortUrl(string url);
}