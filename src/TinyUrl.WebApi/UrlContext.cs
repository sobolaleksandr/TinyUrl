using Microsoft.EntityFrameworkCore;

using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi;

public sealed class UrlContext : DbContext
{
  public UrlContext(DbContextOptions<UrlContext> options)
    : base(options)
  {
    Database.EnsureCreated();
  }

  public DbSet<UrlModel> Urls { get; set; }
}