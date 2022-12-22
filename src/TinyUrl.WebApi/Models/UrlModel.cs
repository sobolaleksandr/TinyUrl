using System.ComponentModel.DataAnnotations;


namespace TinyUrl.WebApi.Models;

public class UrlModel
{
  [Required]
  public Guid Id { get; set; }

  [Url]
  public string FullAddress { get; set; }

  [Url]
  public string ShortAddress { get; set; }
}