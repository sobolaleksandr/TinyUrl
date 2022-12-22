using Microsoft.AspNetCore.Mvc;

using TinyUrl.WebApi.Models;
using TinyUrl.WebApi.Services;


namespace TinyUrl.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
  private readonly ILogger<UrlController> _logger;
  private readonly IUrlService _urlService;

  public UrlController(ILogger<UrlController> logger, IUrlService urlService)
  {
    _logger = logger;
    _urlService = urlService;
  }

  /// <summary>
  /// Поиск ссылки по токену и редирект.
  /// </summary>
  /// <param name="shortUrl">Токен.</param>
  /// <response code="307">Объект найден в базе данных.</response>
  /// <response code="404">Объект отсутствует в базе данных.</response>
  /// <response code="500">Произошло исключение при получении объекта.</response>
  [HttpGet]
  [ProducesResponseType(typeof(UrlModel), StatusCodes.Status302Found)]
  [ProducesResponseType(typeof(RestApiError), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(RestApiError), StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<string>> GetFullUrl(string shortUrl)
  {
    UrlModel? url = await _urlService.GetByUrl(shortUrl);
    if (url == null)
    {
      var notFoundError = new RestApiError
      {
        Message = "Объект не найден."
      };

      return NotFound(notFoundError);
    }

    _logger.LogInformation("Объект успешно получен: {@fullUrl}", url);

    return Redirect(url.FullAddress);
  }

  /// <summary>
  /// Генерация короткого токена.
  /// </summary>
  /// <param name="fullUrl">Ссылка.</param>
  /// <response code="200">Объект найден в базе данных.</response>
  /// <response code="500">Произошло исключение при получении объекта.</response>
  [HttpPost]
  [ProducesResponseType(typeof(UrlModel), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(RestApiError), StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<string>> CreateShortUrl(string fullUrl)
  {
    UrlModel? url = await _urlService.GetByUrl(fullUrl);
    if (url != null)
      return Ok(url.ShortAddress);

    UrlModel newUrl = await _urlService.CreateUrl(fullUrl);

    return Ok(newUrl.ShortAddress);
  }
}