using System.Net.Mime;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Controllers;

/// <summary>
/// Пример обработки исключений для всего сервиса.
/// Логирование ошибок выполняется движком Serilog автоматически и дополнительных действий не требуется.
/// Данный подход нужен лишь для стандартизации ответов в случае появления исключений.
/// 
/// Подробнее об обработке ошибок смотри тут: https://docs.microsoft.com/ru-ru/aspnet/core/web-api/handle-errors
/// </summary>
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
public class ExceptionHandlerController : ControllerBase
{
	[Route("/error")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public ActionResult<RestApiError> HandleError()
	{
		var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

		var error = new RestApiError
		{
			Message = exceptionHandlerFeature.Error.Message
		};

		return StatusCode(StatusCodes.Status500InternalServerError, error);
	}
}