using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ModularityPro.Models
{
  public class ModularityProContextFactory : IDesignTimeDbContextFactory<ModularityProContext>
  {
    ModularityProContext IDesignTimeDbContextFactory<ModularityProContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var builder = new DbContextOptionsBuilder<ModularityProContext>();
      var connectionString = configuration.GetConnectionString("DefaultConnection");

      builder.UseMySql(connectionString);

      return new ModularityProContext(builder.Options);
    }
  }
}