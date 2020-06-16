using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;

namespace ModularityPro.Models
{
  public class ModularityProContext : IdentityDbContext<ApplicationUser>
  {
    public ModularityProContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Friend> Friends { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
  }
}