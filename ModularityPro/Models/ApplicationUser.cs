using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace ModularityPro.Models
{
  public class ApplicationUser : IdentityUser
  {
    public static ModularityProContext _db;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [DataType(DataType.MultilineText)]
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
  }
}