namespace TinyUrl.WebApi.Models;

/// <summary>
/// Пример сообщения об ошибке в случае выполнения операции.
/// Позволяет стандартизировать обработку исключений в вызываемом коде и упрощает взаимодействие.
/// </summary>
public class RestApiError
{
	/// <summary>
	/// Сообщение об ошибке.
	/// </summary>
  public string Message { get; init; }
}