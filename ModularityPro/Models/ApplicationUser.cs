using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ModularityPro.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [DataType(DataType.MultilineText)]
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }

  }
}