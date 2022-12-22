using Microsoft.Extensions.Options;

using TinyUrl.WebApi.Config;
using TinyUrl.WebApi.Models;
using TinyUrl.WebApi.Repositories;


namespace TinyUrl.WebApi.Services;

public class UrlService : IUrlService
{
  private static readonly Random _random = new();
  private readonly ServiceConfig _config;
  private readonly IUrlRepository _urlRepository;

  public UrlService(IOptions<ServiceConfig> options, IUrlRepository urlRepository)
  {
    _urlRepository = urlRepository;
    _config = options.Value;
  }

  public async Task<UrlModel> CreateUrl(string url)
  {
    string shortUrl = await CreateShortUrl();
    var newUrl = new UrlModel
    {
      FullAddress = url,
      ShortAddress = shortUrl
    };

    await _urlRepository.CreateUrl(newUrl);

    return newUrl;
  }

  public async Task<UrlModel?> GetByUrl(string url) => await _urlRepository.GetByUrl(url);

  private async Task<string> CreateShortUrl()
  {
    while (true)
    {
      string shortUrl = $"{_config.ServerUrl}?u={RandomString(_config.UrlLength)}";
      UrlModel? url = await _urlRepository.GetByUrl(shortUrl);
      if (url == null)
        return shortUrl;
    }
  }

  private static string RandomString(int length)
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string(Enumerable.Repeat(chars, length)
      .Select(s => s[_random.Next(s.Length)]).ToArray());
  }
}