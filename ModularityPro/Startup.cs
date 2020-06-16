using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using ModularityPro.Models;
using ModularityPro.Hubs;
using Microsoft.AspNetCore.Session;
using System.Runtime;

namespace ModularityPro
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
      // 
      services.AddMvc();

      services.AddEntityFrameworkMySql()
        .AddDbContext<ModularityProContext>(options => options
        .UseMySql(Configuration["ConnectionStrings:DefaultConnection"]));

      services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ModularityProContext>()
                .AddDefaultTokenProviders();


      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
      });

      services.AddSignalR();

      // services.AddDistributedMemoryCache();
      services.AddMemoryCache();
      // services.AddSingleton<List>();
      services.AddSession();
      // services.AddMemoryCache();

    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseStaticFiles();

      app.UseDeveloperExceptionPage();

      app.UseAuthentication();

      app.UseSession();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });

      app.UseSignalR(routes =>
      {
        routes.MapHub<ChatHub>("/chatHub");
      });
    }
  }
}