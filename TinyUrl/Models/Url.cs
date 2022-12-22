using System.ComponentModel.DataAnnotations;


namespace TinyUrl.WebApi.Models;

public class Url
{
	public Guid Id { get; set; }

	[Url]
	public string FullAddress { get; set; }

	[Url]
	public string ShortAddress { get; set; }
}

/// <summary>
///   Объект на карте.
/// </summary>
public class Feature
{
	/// <summary>
	///   Идентификатор объекта.
	/// </summary>
	[Required]
	[Range(0, int.MaxValue)]
	public int Id { get; init; }

	/// <summary>
	///   Источник объекта.
	/// </summary>
	[Url]
	public string Source { get; init; }

	/// <summary>
	///   Геометрия в формате WKT.
	/// </summary>
	public string? WktGeometry { get; init; }
}