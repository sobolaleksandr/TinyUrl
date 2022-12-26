using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Repositories;

public interface IUrlRepository
{
	Task<UrlModel?> GetByFullUrl(string url);

	Task<UrlModel?> GetByShortUrl(string url);

	Task CreateUrl(UrlModel url);
}