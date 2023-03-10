using IronBarCode;

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
		UrlModel newUrl = await CreateShortUrl(url);
		await _urlRepository.CreateUrl(newUrl);

		return newUrl;
	}

	public async Task<UrlModel?> GetByFullUrl(string url) => await _urlRepository.GetByFullUrl(url);

	public async Task<UrlModel?> GetByShortUrl(string url) => await _urlRepository.GetByShortUrl(url);

	private string CreateQrCode(string shortUrl, string randomString)
	{
		GeneratedBarcode qrCode = QRCodeWriter.CreateQrCode(shortUrl);
		string fileName = @$"{randomString}.jpeg";
		qrCode.SaveAsJpeg($@"{_config.ImagesFolderPath}{fileName}");

		return fileName;
	}

	private async Task<UrlModel> CreateShortUrl(string fullUrl)
	{
		while (true)
		{
			string randomString = RandomString(_config.UrlLength);
			string shortUrl = $"{_config.ServerUrl}{randomString}";
			UrlModel? url = await _urlRepository.GetByFullUrl(shortUrl);
			if (url == null)
				return new UrlModel
				{
					FullAddress = fullUrl,
					ShortAddress = randomString,
					QrCodePath = CreateQrCode(shortUrl, randomString)
				};
		}
	}

	private static string RandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[_random.Next(s.Length)]).ToArray());
	}
}